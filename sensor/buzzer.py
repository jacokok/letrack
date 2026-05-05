try:
    import uasyncio as asyncio
except ImportError:
    import asyncio

from machine import Pin


class Buzzer:
    def __init__(self):
        self.buzzer = Pin(11, Pin.OUT)

    def error(self):
        asyncio.create_task(self.__error())

    def ready(self):
        asyncio.create_task(self.__ready())

    def wifi(self):
        asyncio.create_task(self.__wifi())

    def lap(self):
        asyncio.create_task(self.__lap())

    async def __lap(self):
        self.buzzer.on()
        await asyncio.sleep(0.120)
        self.buzzer.off()

    async def __error(self):
        self.buzzer.on()
        await asyncio.sleep(3)
        self.buzzer.off()

    async def __ready(self):
        self.buzzer.on()
        await asyncio.sleep(0.150)
        self.buzzer.off()
        await asyncio.sleep(0.050)
        self.buzzer.on()
        await asyncio.sleep(0.150)
        self.buzzer.off()
        await asyncio.sleep(0.050)
        self.buzzer.on()
        await asyncio.sleep(0.150)
        self.buzzer.off()

    async def __wifi(self):
        self.buzzer.on()
        await asyncio.sleep(0.150)
        self.buzzer.off()
        await asyncio.sleep(0.150)
        self.buzzer.on()
        await asyncio.sleep(0.150)
        self.buzzer.off()
