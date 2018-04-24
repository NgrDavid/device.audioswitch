#include <avr/io.h>
#include "hwbp_core_types.h"
#include "app_ios_and_regs.h"

/************************************************************************/
/* Configure and initialize IOs                                         */
/************************************************************************/
void init_ios(void)
{	/* Configure input pins */
	io_pin2in(&PORTB, 0, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // IN0
	io_pin2in(&PORTB, 1, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // IN1
	io_pin2in(&PORTB, 2, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // IN2
	io_pin2in(&PORTB, 3, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // IN3
	io_pin2in(&PORTC, 0, PULL_IO_TRISTATE, SENSE_IO_EDGES_BOTH);         // IN4
	io_pin2in(&PORTC, 4, PULL_IO_DOWN, SENSE_IO_EDGES_BOTH);             // ADD

	/* Configure input interrupts */
	io_set_int(&PORTB, INT_LEVEL_LOW, 0, (1<<0), false);                 // IN0
	io_set_int(&PORTB, INT_LEVEL_LOW, 0, (1<<1), false);                 // IN1
	io_set_int(&PORTB, INT_LEVEL_LOW, 0, (1<<2), false);                 // IN2
	io_set_int(&PORTB, INT_LEVEL_LOW, 0, (1<<3), false);                 // IN3
	io_set_int(&PORTC, INT_LEVEL_LOW, 0, (1<<0), false);                 // IN4
	io_set_int(&PORTC, INT_LEVEL_LOW, 1, (1<<4), false);                 // ADD

	/* Configure output pins */
	io_pin2out(&PORTA, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN0
	io_pin2out(&PORTA, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN1
	io_pin2out(&PORTA, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN2
	io_pin2out(&PORTA, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN3
	io_pin2out(&PORTA, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN4
	io_pin2out(&PORTA, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN5
	io_pin2out(&PORTA, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN6
	io_pin2out(&PORTA, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN7
	io_pin2out(&PORTD, 0, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN8
	io_pin2out(&PORTD, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN9
	io_pin2out(&PORTD, 2, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN10
	io_pin2out(&PORTD, 3, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN11
	io_pin2out(&PORTD, 4, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN12
	io_pin2out(&PORTD, 5, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN13
	io_pin2out(&PORTD, 6, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN14
	io_pin2out(&PORTD, 7, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // EN15
	io_pin2out(&PORTC, 1, OUT_IO_DIGITAL, IN_EN_IO_EN);                  // DO0

	/* Initialize output pins */
	clr_EN0;
	clr_EN1;
	clr_EN2;
	clr_EN3;
	clr_EN4;
	clr_EN5;
	clr_EN6;
	clr_EN7;
	clr_EN8;
	clr_EN9;
	clr_EN10;
	clr_EN11;
	clr_EN12;
	clr_EN13;
	clr_EN14;
	clr_EN15;
	clr_DO0;
}

/************************************************************************/
/* Registers' stuff                                                     */
/************************************************************************/
AppRegs app_regs;

uint8_t app_regs_type[] = {
	TYPE_U8,
	TYPE_U16,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8,
	TYPE_U8
};

uint16_t app_regs_n_elements[] = {
	1,
	1,
	1,
	1,
	1,
	1,
	1,
	1
};

uint8_t *app_regs_pointer[] = {
	(uint8_t*)(&app_regs.REG_SOURCE),
	(uint8_t*)(&app_regs.REG_CHANNEL_SEL),
	(uint8_t*)(&app_regs.REG_DI_STATE),
	(uint8_t*)(&app_regs.REG_DO),
	(uint8_t*)(&app_regs.REG_RESERVED0),
	(uint8_t*)(&app_regs.REG_DI4_CONF),
	(uint8_t*)(&app_regs.REG_DO0_CONF),
	(uint8_t*)(&app_regs.REG_EVNT_ENABLE)
};