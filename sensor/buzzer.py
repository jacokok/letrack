# pyright: reportImplicitRelativeImport=false
try:
    import uasyncio as asyncio
except ImportError:
    import asyncio

try:
    from . import config as conf
except ImportError:
    try:
        import config as conf
    except ImportError:
        conf = None

from machine import Pin


class Buzzer:
    def __init__(self):
        self.buzzer = Pin(11, Pin.OUT)

    def _muted(self):
        return bool(getattr(conf, "MUTE_SPEAKER", False))

    def error(self):
        if not self._muted():
            asyncio.create_task(self.__error())

    def ready(self):
        if not self._muted():
            asyncio.create_task(self.__ready())

    def wifi(self):
        if not self._muted():
            asyncio.create_task(self.__wifi())

    def lap(self):
        if not self._muted():
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
