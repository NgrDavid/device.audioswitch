#include "app_funcs.h"
#include "app_ios_and_regs.h"
#include "hwbp_core.h"

#define F_CPU 32000000
#include <util\delay.h>

/************************************************************************/
/* Create pointers to functions                                         */
/************************************************************************/
extern AppRegs app_regs;

void (*app_func_rd_pointer[])(void) = {
	&app_read_REG_SOURCE,
	&app_read_REG_CHANNEL_SEL,
	&app_read_REG_DI_STATE,
	&app_read_REG_DO,
	&app_read_REG_RESERVED0,
	&app_read_REG_DI4_CONF,
	&app_read_REG_DO0_CONF,
	&app_read_REG_EVNT_ENABLE
};

bool (*app_func_wr_pointer[])(void*) = {
	&app_write_REG_SOURCE,
	&app_write_REG_CHANNEL_SEL,
	&app_write_REG_DI_STATE,
	&app_write_REG_DO,
	&app_write_REG_RESERVED0,
	&app_write_REG_DI4_CONF,
	&app_write_REG_DO0_CONF,
	&app_write_REG_EVNT_ENABLE
};


void update_outputs(bool update_DO0, bool from_address_interrupt)
{
   uint16_t current_state, new_state;
   
   *(((uint8_t*)(&current_state)) + 0) = PORTA_IN;
   *(((uint8_t*)(&current_state)) + 1) = PORTD_IN;
   
   if (app_regs.REG_SOURCE == GM_USB)
   {
      if ((app_regs.REG_DI4_CONF == GM_DI4_DIGITAL) | ((app_regs.REG_DI4_CONF == GM_DI4_ADDRESS) && ((read_ADD ? true : false) == (read_IN4 ? true : false))))
      {
         PORTA_OUT = 0;
         PORTD_OUT = 0;
         
         _delay_us(100);
         
         PORTA_OUT = *(((uint8_t*)(&app_regs.REG_CHANNEL_SEL)) + 0);
         PORTD_OUT = *(((uint8_t*)(&app_regs.REG_CHANNEL_SEL)) + 1);
      }
      else
      {
         PORTA_OUT = 0;
         PORTD_OUT = 0;         
      }
   }
   else // app_regs.REG_SOURCE == GM_EXTERNAL
   {
      if ((app_regs.REG_DI4_CONF == GM_DI4_DIGITAL) | ((app_regs.REG_DI4_CONF == GM_DI4_ADDRESS) && ((read_ADD ? true : false) == (read_IN4 ? true : false))))
      {
         PORTA_OUT = 0;
         PORTD_OUT = 0;
         
         _delay_us(100);
         
         uint8_t channel = PORTB_IN & 0x0F;
         
         if (channel <= 7)
         {
            PORTA_OUT = (1 << channel);
         }
         else
         {
            PORTD_OUT = (1 << (channel - 8));
         }
      }
      else
      {
         PORTA_OUT = 0;
         PORTD_OUT = 0;         
      }
   }
   
   *(((uint8_t*)(&new_state)) + 0) = PORTA_IN;
   *(((uint8_t*)(&new_state)) + 1) = PORTD_IN;   
   
   if (current_state != new_state)
   {
      if (app_regs.REG_DO0_CONF == GM_DO_TGL_WHEN_CH_SEL_CHANGE)
      {
         if (update_DO0)
         {
            tgl_DO0;
         }
      }         
                     
      if ((app_regs.REG_SOURCE == GM_EXTERNAL) && (app_regs.REG_EVNT_ENABLE & B_EVT_OUTPUT_CHANNEL))
      {
         app_regs.REG_CHANNEL_SEL = new_state;
         core_func_send_event(ADD_REG_CHANNEL_SEL, true);
      }
      
      if ((app_regs.REG_SOURCE == GM_USB) && from_address_interrupt)
      {
         uint16_t temporary = app_regs.REG_CHANNEL_SEL;
         
         *(((uint8_t*)(&app_regs.REG_CHANNEL_SEL)) + 0) = PORTA_IN;
         *(((uint8_t*)(&app_regs.REG_CHANNEL_SEL)) + 1) = PORTD_IN;
         
         core_func_send_event(ADD_REG_CHANNEL_SEL, true);
         
         app_regs.REG_CHANNEL_SEL = temporary;
      }
   }
}

/************************************************************************/
/* REG_SOURCE                                                           */
/************************************************************************/
void app_read_REG_SOURCE(void) {}
bool app_write_REG_SOURCE(void *a)
{
   uint16_t reg = *((uint8_t*)a);
   
   if (reg & ~MSK_SOURCE)
      return false;

   if (reg != app_regs.REG_SOURCE)
   {
      update_outputs(true, false);
   }
   
   app_regs.REG_SOURCE = *((uint8_t*)a);
   return true;
}



/************************************************************************/
/* REG_CHANNEL_SEL                                                      */
/************************************************************************/
void app_read_REG_CHANNEL_SEL(void)
{
	if (app_regs.REG_SOURCE == GM_EXTERNAL)
   {
      *(((uint8_t*)(&app_regs.REG_CHANNEL_SEL)) + 0) = PORTA_IN;
      *(((uint8_t*)(&app_regs.REG_CHANNEL_SEL)) + 1) = PORTD_IN;
   }      
}

bool app_write_REG_CHANNEL_SEL(void *a)
{
	uint16_t reg = *((uint16_t*)a);
   
   if (app_regs.REG_SOURCE != GM_USB)
   {
      return false;
   }
         
   if (reg != app_regs.REG_CHANNEL_SEL)
   {
      app_regs.REG_CHANNEL_SEL = reg;
      update_outputs(true, false);
   }
   
   return true;
}


/************************************************************************/
/* REG_DI_STATE                                                         */
/************************************************************************/
void app_read_REG_DI_STATE(void)
{
   app_regs.REG_DI_STATE = (PORTB_IN & 0x0F) | (read_IN4 ? B_DI4 : 0);
}

bool app_write_REG_DI_STATE(void *a)
{
   return false;
}


/************************************************************************/
/* REG_DO                                                               */
/************************************************************************/
void app_read_REG_DO(void)
{
   app_regs.REG_DO = read_DO0 ? B_DO0 : 0;
}

bool app_write_REG_DO(void *a)
{
   uint8_t reg = *((uint8_t*)a);
   
   if (reg & ~B_DO0)
      return false;
      
   if (reg & B_DO0)
   {
      set_DO0;
   }
   else
   {
      clr_DO0;
   }

   app_regs.REG_DO = reg;
   return true;
}


/************************************************************************/
/* REG_RESERVED0                                                        */
/************************************************************************/
void app_read_REG_RESERVED0(void) {}
bool app_write_REG_RESERVED0(void *a) { return true; }


/************************************************************************/
/* REG_DI4_CONF                                                         */
/************************************************************************/
void app_read_REG_DI4_CONF(void) {}
bool app_write_REG_DI4_CONF(void *a)
{
   if (*((uint8_t*)a) & ~MSK_DI4_CONF)
      return false;

   app_regs.REG_DI4_CONF = *((uint8_t*)a);
   update_outputs(true, false);
   return true;
}


/************************************************************************/
/* REG_DO0_CONF                                                         */
/************************************************************************/
void app_read_REG_DO0_CONF(void) {}
bool app_write_REG_DO0_CONF(void *a)
{
   if (*((uint8_t*)a) & ~MSK_DO0_CONF)
      return false;

   app_regs.REG_DO0_CONF = *((uint8_t*)a);
   return true;
}


/************************************************************************/
/* REG_EVNT_ENABLE                                                      */
/************************************************************************/
void app_read_REG_EVNT_ENABLE(void) {}
bool app_write_REG_EVNT_ENABLE(void *a)
{
	app_regs.REG_EVNT_ENABLE = *((uint8_t*)a);
	return true;
}