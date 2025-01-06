import uasyncio
from machine import Pin
from buzzer import Buzzer
from track import BreakBeam
from umqtt.robust import MQTTClient
import config
import ntptime
import time


async def beam_handler(beam: BreakBeam, mqtt: MQTTClient):
    while True:
        uasyncio.create_task(beam.check(mqtt))
        await uasyncio.sleep_ms(0)


async def main():
    try:
        buzzer = Buzzer()
        ntptime.host = config.NTP_HOST
        ntptime.settime()

        mqtt = MQTTClient(
            "letrack",
            config.MQTT_SERVER,
            keepalive=30000,
            ssl=False,
        )
        mqtt.connect()
        beam1 = Pin(10, Pin.IN, Pin.PULL_UP)
        beam2 = Pin(12, Pin.IN, Pin.PULL_UP)
        bb1 = BreakBeam(beam1, 1)
        bb2 = BreakBeam(beam2, 2)

        buzzer.ready()

        while True:
            uasyncio.Loop.run_until_complete(
                uasyncio.create_task(beam_handler(bb1, mqtt))
            )
            uasyncio.Loop.run_until_complete(
                uasyncio.create_task(beam_handler(bb2, mqtt))
            )

    except Exception as e:
        print("Error occurred", e)
        buzzer.error()


uasyncio.run(main())
