import uasyncio
from machine import Pin
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
        # ntptime.host = "1.europe.pool.ntp.org"
        ntptime.host = "192.168.1.29"
        ntptime.settime()
        print("Local time after synchronization：%s" % str(time.localtime()))
    except:
        print("Error syncing time")

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
    while True:
        uasyncio.create_task(beam_handler(bb1, mqtt))
        uasyncio.create_task(beam_handler(bb2, mqtt))
        await uasyncio.sleep(10_000)


uasyncio.run(main())
