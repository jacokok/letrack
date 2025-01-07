from sys import platform, implementation
from mqtt_as import config
from machine import Pin
import config as conf

config["server"] = conf.MQTT_SERVER
config["ssid"] = conf.WIFI_SSID
config["wifi_pw"] = conf.WIFI_PASSWD
config["client_id"] = "letrack"


def ledfunc(pin):
    pin = pin

    def func(v):
        pin(v)

    return func


wifi_led = lambda _: None  # Only one LED
LED = "LED" if "Pico W" in implementation._machine else 25
blue_led = ledfunc(Pin(LED, Pin.OUT, value=0))  # Message received
