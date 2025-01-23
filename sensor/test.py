import uasyncio
from machine import Pin
from buzzer import Buzzer
from track import BreakBeam
from mqtt_as import MQTTClient
from mqtt_local import config as mqtt_config
import network
import config
import ntptime
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
    uasyncio.run(main())
except Exception as e:
    buzzer = Buzzer()
    print("Error occurred", e)
    light.off()
    buzzer.error()
finally:
    light.off()
    uasyncio.new_event_loop()
    print("Finallyh")
