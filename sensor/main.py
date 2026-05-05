# pyright: reportImplicitRelativeImport=false
try:
    import uasyncio as asyncio
except ImportError:
    import asyncio

import config
import network
import ntptime
from buzzer import Buzzer
from machine import Pin
from mqtt_as import MQTTClient
from mqtt_local import config as mqtt_config
from track import BreakBeam

light = Pin(6, Pin.OUT)
buzzer = Buzzer()


async def beam_handler(beam1, beam2, mqtt):
    while True:
        await beam1.check(mqtt)
        await beam2.check(mqtt)
        await asyncio.sleep(0.01)


async def down(client):
    while True:
        await client.down.wait()
        client.down.clear()
        buzzer.error()
        light.off()


async def up(client):
    while True:
        await client.up.wait()
        client.up.clear()
        buzzer.ready()
        light.on()
        # This is where you could subscribe to topics
        # await client.subscribe("foo_topic", 1)


async def connect_wifi(buzzer):
    light.off()
    ap_if = network.WLAN(network.AP_IF)
    ap_if.config(hostname=config.HOSTNAME)
    network.hostname(config.HOSTNAME)
    ap_if.active(False)
    sta_if = network.WLAN(network.STA_IF)
    if not sta_if.isconnected():
        # print("Connecting to WiFi...")
        sta_if.active(True)
        sta_if.config(hostname=config.HOSTNAME)
        sta_if.connect(config.WIFI_SSID, config.WIFI_PASSWD)
        while not sta_if.isconnected():
            light.on()
            await asyncio.sleep(1)
            light.off()
    # print("Network config:", sta_if.ifconfig())
    buzzer.wifi()


async def main(client):
    led = Pin("LED", Pin.OUT)
    led.on()
    light.off()

    try:
        ntptime.host = config.NTP_HOST
        ntptime.settime()
        mqtt_config["client_id"] = config.HOSTNAME
        await client.connect()
    except OSError:
        light.off()
        buzzer.error()
        print("MQTT Connection failed.")
        return

    asyncio.create_task(up(client))
    asyncio.create_task(down(client))

    beam1 = Pin(10, Pin.IN, Pin.PULL_UP)
    beam2 = Pin(12, Pin.IN, Pin.PULL_UP)
    bb1 = BreakBeam(
        beam1,
        config.BEAM1,
        config.SAME_LANE_COOLDOWN_MS,
        config.CROSS_LANE_SUPPRESSION_MS,
    )
    bb2 = BreakBeam(
        beam2,
        config.BEAM2,
        config.SAME_LANE_COOLDOWN_MS,
        config.CROSS_LANE_SUPPRESSION_MS,
    )

    await beam_handler(bb1, bb2, client)


client = None

try:
    asyncio.run(connect_wifi(buzzer))
    # MQTTClient.DEBUG = True
    mqtt_config["keepalive"] = 120
    mqtt_config["queue_len"] = 1
    # Can include last will
    # mqtt_config["will"] = ("status", "Goodbye cruel world!", False, 0)
    client = MQTTClient(mqtt_config)
    asyncio.run(main(client))
except Exception as e:
    print("Error occurred", e)
    light.off()
    buzzer.error()
finally:
    if client is not None:
        client.close()
    light.off()
    try:
        asyncio.new_event_loop()
    except AttributeError:
        pass
    print("Finished")
