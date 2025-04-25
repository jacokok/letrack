import uasyncio
from machine import Pin
from buzzer import Buzzer
from track import BreakBeam
from mqtt_as import MQTTClient
from mqtt_local import config as mqtt_config
import network
import config
import ntptime

light = Pin(6, Pin.OUT)
buzzer = Buzzer()


async def beam_handler(beam1: BreakBeam, beam2: BreakBeam, mqtt: MQTTClient):
    while True:
        uasyncio.create_task(beam1.check(mqtt))
        uasyncio.create_task(beam2.check(mqtt))
        await uasyncio.sleep_ms(0)


async def down(client: MQTTClient):
    while True:
        await client.down.wait()
        client.down.clear()
        buzzer.error()
        light.off()


async def up(client: MQTTClient):
    while True:
        await client.up.wait()
        client.up.clear()
        buzzer.ready()
        light.on()
        # This is where you could subscribe to topics
        # await client.subscribe("foo_topic", 1)


async def connect_wifi(buzzer: Buzzer):
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
            uasyncio.sleep_ms(1000)
            light.off()
    # print("Network config:", sta_if.ifconfig())
    buzzer.wifi()


async def main(client: MQTTClient):
    led = Pin("LED", Pin.OUT)
    led.on()
    light.off()

    try:
        ntptime.host = config.NTP_HOST
        ntptime.settime()
        client._client_id = config.HOSTNAME
        await client.connect()
    except OSError:
        light.off()
        buzzer.error()
        print("MQTT Connection failed.")

    uasyncio.create_task(up(client))
    uasyncio.create_task(down(client))

    beam1 = Pin(10, Pin.IN, Pin.PULL_UP)
    beam2 = Pin(12, Pin.IN, Pin.PULL_UP)
    bb1 = BreakBeam(beam1, config.BEAM1)
    bb2 = BreakBeam(beam2, config.BEAM2)

    uasyncio.run(beam_handler(bb1, bb2, client))


try:
    uasyncio.run(connect_wifi(buzzer))
    # MQTTClient.DEBUG = True
    mqtt_config["keepalive"] = 120
    mqtt_config["queue_len"] = 1
    # Can include last will
    # mqtt_config["will"] = ("status", "Goodbye cruel world!", False, 0)
    client = MQTTClient(mqtt_config)
    uasyncio.run(main(client))
except Exception as e:
    print("Error occurred", e)
    light.off()
    buzzer.error()
finally:
    client.close()
    light.off()
    uasyncio.new_event_loop()
    print("Finished")
