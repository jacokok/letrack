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
from utime import sleep_ms

light = Pin(6, Pin.OUT)


def connect_wifi():
    ap_if = network.WLAN(network.AP_IF)
    ap_if.active(False)
    sta_if = network.WLAN(network.STA_IF)
    if not sta_if.isconnected():
        print("Connecting to WiFi...")
        sta_if.active(True)
        sta_if.connect(config.WIFI_SSID, config.WIFI_PASSWD)
        while not sta_if.isconnected():
            sleep_ms(1000)

    print("Network config:", sta_if.ifconfig())


async def main():
    led = Pin("LED", Pin.OUT)
    led.on()
    light.off()
    print("main running")
    buzzer = Buzzer()

    try:
        connect_wifi()

        ntptime.host = config.NTP_HOST
        ntptime.settime()

    except OSError:
        print("Connection failed.")
        light.off()
        buzzer.error()

    print("it is done")


try:
    asyncio.run(main())
except Exception as e:
    buzzer = Buzzer()
    print("Error occurred", e)
    light.off()
    buzzer.error()
finally:
    light.off()
    try:
        asyncio.new_event_loop()
    except AttributeError:
        pass
    print("Finally")
