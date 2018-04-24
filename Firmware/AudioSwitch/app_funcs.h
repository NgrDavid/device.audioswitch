#ifndef _APP_FUNCTIONS_H_
#define _APP_FUNCTIONS_H_
#include <avr/io.h>


/************************************************************************/
/* Define if not defined                                                */
/************************************************************************/
#ifndef bool
	#define bool uint8_t
#endif
#ifndef true
	#define true 1
#endif
#ifndef false
	#define false 0
#endif


/************************************************************************/
/* Prototypes                                                           */
/************************************************************************/
void app_read_REG_SOURCE(void);
void app_read_REG_CHANNEL_SEL(void);
void app_read_REG_DI_STATE(void);
void app_read_REG_DO(void);
void app_read_REG_RESERVED0(void);
void app_read_REG_DI4_CONF(void);
void app_read_REG_DO0_CONF(void);
void app_read_REG_EVNT_ENABLE(void);

bool app_write_REG_SOURCE(void *a);
bool app_write_REG_CHANNEL_SEL(void *a);
bool app_write_REG_DI_STATE(void *a);
bool app_write_REG_DO(void *a);
bool app_write_REG_RESERVED0(void *a);
bool app_write_REG_DI4_CONF(void *a);
bool app_write_REG_DO0_CONF(void *a);
bool app_write_REG_EVNT_ENABLE(void *a);


#endif /* _APP_FUNCTIONS_H_ */