# pyright: reportImplicitRelativeImport=false
from sys import implementation

import config as conf
from machine import Pin


def subs_cb(*_):
    return None


async def no_op(*_):
    return None


config = {
    "client_id": "letrack",
    "server": conf.MQTT_SERVER,
    "port": 0,
    "user": "",
    "password": "",
    "keepalive": 60,
    "ping_interval": 0,
    "ssl": False,
    "ssl_params": {},
    "response_time": 10,
    "clean_init": True,
    "clean": True,
    "max_repubs": 4,
    "will": None,
    "subs_cb": subs_cb,
    "wifi_coro": no_op,
    "connect_coro": no_op,
    "ssid": conf.WIFI_SSID,
    "wifi_pw": conf.WIFI_PASSWD,
    "queue_len": 0,
    "gateway": False,
    "mqttv5": False,
    "mqttv5_con_props": None,
}


def ledfunc(pin):
    def func(v):
        pin(v)

    return func


def wifi_led(_):
    return None


machine_name = str(getattr(implementation, "_machine", ""))
LED = "LED" if "Pico W" in machine_name else 25
blue_led = ledfunc(Pin(LED, Pin.OUT, value=0))  # Message received
