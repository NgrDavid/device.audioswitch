#ifndef _APP_IOS_AND_REGS_H_
#define _APP_IOS_AND_REGS_H_
#include "cpu.h"

void init_ios(void);
/************************************************************************/
/* Definition of input pins                                             */
/************************************************************************/
// IN0                    Description: Decoder bit 0
// IN1                    Description: Decoder bit 1
// IN2                    Description: Decoder bit 2
// IN3                    Description: Decoder bit 3
// IN4                    Description: Digital Input 4
// ADD                    Description: Address

#define read_IN0 read_io(PORTB, 0)              // IN0
#define read_IN1 read_io(PORTB, 1)              // IN1
#define read_IN2 read_io(PORTB, 2)              // IN2
#define read_IN3 read_io(PORTB, 3)              // IN3
#define read_IN4 read_io(PORTC, 0)              // IN4
#define read_ADD read_io(PORTC, 4)              // ADD

/************************************************************************/
/* Definition of output pins                                            */
/************************************************************************/
// EN0                    Description: Enable channel 0
// EN1                    Description: Enable channel 1
// EN2                    Description: Enable channel 2
// EN3                    Description: Enable channel 3
// EN4                    Description: Enable channel 4
// EN5                    Description: Enable channel 5
// EN6                    Description: Enable channel 6
// EN7                    Description: Enable channel 7
// EN8                    Description: Enable channel 8
// EN9                    Description: Enable channel 9
// EN10                   Description: Enable channel 10
// EN11                   Description: Enable channel 11
// EN12                   Description: Enable channel 12
// EN13                   Description: Enable channel 13
// EN14                   Description: Enable channel 14
// EN15                   Description: Enable channel 15
// DO0                    Description: Digital output 0

/* EN0 */
#define set_EN0 set_io(PORTA, 0)
#define clr_EN0 clear_io(PORTA, 0)
#define tgl_EN0 toggle_io(PORTA, 0)
#define read_EN0 read_io(PORTA, 0)

/* EN1 */
#define set_EN1 set_io(PORTA, 1)
#define clr_EN1 clear_io(PORTA, 1)
#define tgl_EN1 toggle_io(PORTA, 1)
#define read_EN1 read_io(PORTA, 1)

/* EN2 */
#define set_EN2 set_io(PORTA, 2)
#define clr_EN2 clear_io(PORTA, 2)
#define tgl_EN2 toggle_io(PORTA, 2)
#define read_EN2 read_io(PORTA, 2)

/* EN3 */
#define set_EN3 set_io(PORTA, 3)
#define clr_EN3 clear_io(PORTA, 3)
#define tgl_EN3 toggle_io(PORTA, 3)
#define read_EN3 read_io(PORTA, 3)

/* EN4 */
#define set_EN4 set_io(PORTA, 4)
#define clr_EN4 clear_io(PORTA, 4)
#define tgl_EN4 toggle_io(PORTA, 4)
#define read_EN4 read_io(PORTA, 4)

/* EN5 */
#define set_EN5 set_io(PORTA, 5)
#define clr_EN5 clear_io(PORTA, 5)
#define tgl_EN5 toggle_io(PORTA, 5)
#define read_EN5 read_io(PORTA, 5)

/* EN6 */
#define set_EN6 set_io(PORTA, 6)
#define clr_EN6 clear_io(PORTA, 6)
#define tgl_EN6 toggle_io(PORTA, 6)
#define read_EN6 read_io(PORTA, 6)

/* EN7 */
#define set_EN7 set_io(PORTA, 7)
#define clr_EN7 clear_io(PORTA, 7)
#define tgl_EN7 toggle_io(PORTA, 7)
#define read_EN7 read_io(PORTA, 7)

/* EN8 */
#define set_EN8 set_io(PORTD, 0)
#define clr_EN8 clear_io(PORTD, 0)
#define tgl_EN8 toggle_io(PORTD, 0)
#define read_EN8 read_io(PORTD, 0)

/* EN9 */
#define set_EN9 set_io(PORTD, 1)
#define clr_EN9 clear_io(PORTD, 1)
#define tgl_EN9 toggle_io(PORTD, 1)
#define read_EN9 read_io(PORTD, 1)

/* EN10 */
#define set_EN10 set_io(PORTD, 2)
#define clr_EN10 clear_io(PORTD, 2)
#define tgl_EN10 toggle_io(PORTD, 2)
#define read_EN10 read_io(PORTD, 2)

/* EN11 */
#define set_EN11 set_io(PORTD, 3)
#define clr_EN11 clear_io(PORTD, 3)
#define tgl_EN11 toggle_io(PORTD, 3)
#define read_EN11 read_io(PORTD, 3)

/* EN12 */
#define set_EN12 set_io(PORTD, 4)
#define clr_EN12 clear_io(PORTD, 4)
#define tgl_EN12 toggle_io(PORTD, 4)
#define read_EN12 read_io(PORTD, 4)

/* EN13 */
#define set_EN13 set_io(PORTD, 5)
#define clr_EN13 clear_io(PORTD, 5)
#define tgl_EN13 toggle_io(PORTD, 5)
#define read_EN13 read_io(PORTD, 5)

/* EN14 */
#define set_EN14 set_io(PORTD, 6)
#define clr_EN14 clear_io(PORTD, 6)
#define tgl_EN14 toggle_io(PORTD, 6)
#define read_EN14 read_io(PORTD, 6)

/* EN15 */
#define set_EN15 set_io(PORTD, 7)
#define clr_EN15 clear_io(PORTD, 7)
#define tgl_EN15 toggle_io(PORTD, 7)
#define read_EN15 read_io(PORTD, 7)

/* DO0 */
#define set_DO0 set_io(PORTC, 1)
#define clr_DO0 clear_io(PORTC, 1)
#define tgl_DO0 toggle_io(PORTC, 1)
#define read_DO0 read_io(PORTC, 1)


/************************************************************************/
/* Registers' structure                                                 */
/************************************************************************/
typedef struct
{
	uint8_t REG_SOURCE;
	uint16_t REG_CHANNEL_SEL;
	uint8_t REG_DI_STATE;
	uint8_t REG_DO;
	uint8_t REG_RESERVED0;
	uint8_t REG_DI4_CONF;
	uint8_t REG_DO0_CONF;
	uint8_t REG_EVNT_ENABLE;
} AppRegs;

/************************************************************************/
/* Registers' address                                                   */
/************************************************************************/
/* Registers */
#define ADD_REG_SOURCE                      32 // U8     Source of the board control
#define ADD_REG_CHANNEL_SEL                 33 // U16    Enable bit mask of the outputs
#define ADD_REG_DI_STATE                    34 // U8     State of the digital inputs
#define ADD_REG_DO                          35 // U8     State of the digital output 0
#define ADD_REG_RESERVED0                   36 // U8     Reserved for future use
#define ADD_REG_DI4_CONF                    37 // U8     Configuration of digital input 4 (DI4)functionality
#define ADD_REG_DO0_CONF                    38 // U8     Configuration of the digital output 0 (DO0) funtionality
#define ADD_REG_EVNT_ENABLE                 39 // U8     Enable the Events

/************************************************************************/
/* PWM Generator registers' memory limits                               */
/*                                                                      */
/* DON'T change the APP_REGS_ADD_MIN value !!!                          */
/* DON'T change these names !!!                                         */
/************************************************************************/
/* Memory limits */
#define APP_REGS_ADD_MIN                    0x20
#define APP_REGS_ADD_MAX                    0x27
#define APP_NBYTES_OF_REG_BANK              9

/************************************************************************/
/* Registers' bits                                                      */
/************************************************************************/
#define MSK_SOURCE                         (1<<0)       // 
#define GM_USB                             (0<<0)       // Device is controlled by a host computer
#define GM_EXTERNAL                        (1<<0)       // Device is  controlled by the digital inputs
#define B_DI0                              (1<<0)       // 
#define B_DI1                              (1<<1)       // 
#define B_DI2                              (1<<2)       // 
#define B_DI3                              (1<<3)       // 
#define B_DI4                              (1<<4)       // 
#define B_DO0                              (1<<0)       // 
#define MSK_DI4_CONF                       (1<<0)       // 
#define GM_DI4_DIGITAL                     (0<<0)       // 
#define GM_DI4_ADDRESS                     (1<<0)       // 
#define MSK_DO0_CONF                       (1<<0)       // 
#define GM_DO_DIGITAL                      (0<<0)       // 
#define GM_DO_TGL_WHEN_CH_SEL_CHANGE       (1<<0)       // 
#define B_EVT_OUTPUT_CHANNEL               (1<<0)       // Event of register CHANNEL_SEL
#define B_EVT_DI_STATE                     (1<<1)       // Event of register DI_STATE

#endif /* _APP_REGS_H_ */