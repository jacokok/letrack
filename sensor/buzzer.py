from machine import Pin
import uasyncio


class Buzzer:
    def __init__(self):
        self.buzzer = Pin(11, Pin.OUT)

    def error(self):
        uasyncio.create_task(self.__error())

    def ready(self):
        uasyncio.create_task(self.__ready())

    def wifi(self):
        uasyncio.create_task(self.__wifi())

    def lap(self):
        uasyncio.create_task(self.__lap())

    async def __lap(self):
        self.buzzer.on()
        await uasyncio.sleep_ms(120)
        self.buzzer.off()

    async def __error(self):
        self.buzzer.on()
        await uasyncio.sleep_ms(3000)
        self.buzzer.off()

    async def __ready(self):
        self.buzzer.on()
        await uasyncio.sleep_ms(150)
        self.buzzer.off()
        await uasyncio.sleep_ms(50)
        self.buzzer.on()
        await uasyncio.sleep_ms(150)
        self.buzzer.off()
        await uasyncio.sleep_ms(50)
        self.buzzer.on()
        await uasyncio.sleep_ms(150)
        self.buzzer.off()

    async def __wifi(self):
        self.buzzer.on()
        await uasyncio.sleep_ms(150)
        self.buzzer.off()
        await uasyncio.sleep_ms(150)
        self.buzzer.on()
        await uasyncio.sleep_ms(150)
        self.buzzer.off()
