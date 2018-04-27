#include "cpu.h"
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"
#include "app_funcs.h"
#include "hwbp_core.h"

/************************************************************************/
/* Declare application registers                                        */
/************************************************************************/
extern AppRegs app_regs;

extern void update_outputs(bool update_DO0, bool from_address_interrupt);

/************************************************************************/
/* Interrupts from Timers                                               */
/************************************************************************/
// ISR(TCC0_OVF_vect, ISR_NAKED)
// ISR(TCD0_OVF_vect, ISR_NAKED)
// ISR(TCE0_OVF_vect, ISR_NAKED)
// ISR(TCF0_OVF_vect, ISR_NAKED)
// 
// ISR(TCC0_CCA_vect, ISR_NAKED)
// ISR(TCD0_CCA_vect, ISR_NAKED)
// ISR(TCE0_CCA_vect, ISR_NAKED)
// ISR(TCF0_CCA_vect, ISR_NAKED)
// 
// ISR(TCD1_OVF_vect, ISR_NAKED)
// 
// ISR(TCD1_CCA_vect, ISR_NAKED)

/************************************************************************/ 
/* IN0-3                                                                */
/************************************************************************/
ISR(PORTB_INT0_vect, ISR_NAKED)
{
   if (app_regs.REG_SOURCE == GM_USB)
   {
      uint8_t reg_di_state = app_regs.REG_DI_STATE;
      
      app_read_REG_DI_STATE();
      
      if (reg_di_state != app_regs.REG_DI_STATE)
      {
         core_func_send_event(ADD_REG_DI_STATE, true);
      }
   }
   else
   {
      update_outputs(true, false);
   }
   
	reti();
}

/************************************************************************/ 
/* IN4                                                                  */
/************************************************************************/
ISR(PORTC_INT0_vect, ISR_NAKED)
{
   if (app_regs.REG_DI4_CONF == GM_DI4_DIGITAL)
   {
      uint8_t reg_di_state = app_regs.REG_DI_STATE;
      
      app_read_REG_DI_STATE();
      
      if (reg_di_state != app_regs.REG_DI_STATE)
      {
         core_func_send_event(ADD_REG_DI_STATE, true);
      }
   }
   else
   {
      update_outputs(true, true);
   }
      
	reti();
}

/************************************************************************/ 
/* ADD                                                                  */
/************************************************************************/
ISR(PORTC_INT1_vect, ISR_NAKED)
{
   update_outputs(true, true);
   
	reti();
}

