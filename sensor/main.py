import uasyncio
from machine import Pin
from buzzer import Buzzer
from track import BreakBeam

from mqtt_as import MQTTClient
from mqtt_local import config as mqtt_config

import config
import ntptime

light = Pin(5, Pin.OUT)


async def beam_handler(beam: BreakBeam, mqtt: MQTTClient):
    while True:
        uasyncio.create_task(beam.check(mqtt))
        await uasyncio.sleep_ms(0)


async def down(client):
    buzzer = Buzzer()
    while True:
        await client.down.wait()
        client.down.clear()
        print("WiFi or broker is down.")
        buzzer.error()


async def up(client):
    buzzer = Buzzer()
    while True:
        await client.up.wait()
        client.up.clear()
        print("We are connected to broker.")
        buzzer.ready()
        # This is where you could subscribe to topics
        # await client.subscribe("foo_topic", 1)


async def main():
    try:
        led = Pin("LED", Pin.OUT)
        led.on()
        buzzer = Buzzer()
        ntptime.host = config.NTP_HOST
        ntptime.settime()

        # Can include last will
        # mqtt_config["will"] = ("status", "Goodbye cruel world!", False, 0)

        client = MQTTClient(mqtt_config)
        await client.connect()

        light.on()

        for task in (up, down):
            uasyncio.create_task(task(client))

        beam1 = Pin(10, Pin.IN, Pin.PULL_UP)
        beam2 = Pin(12, Pin.IN, Pin.PULL_UP)
        bb1 = BreakBeam(beam1, 1)
        bb2 = BreakBeam(beam2, 2)

        while True:
            uasyncio.Loop.run_until_complete(
                uasyncio.create_task(beam_handler(bb1, client))
            )
            uasyncio.Loop.run_until_complete(
                uasyncio.create_task(beam_handler(bb2, client))
            )

    except Exception as e:
        print("Error occurred", e)
        light.off()
        buzzer.error()


uasyncio.run(main())
