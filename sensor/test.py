from machine import Pin
from time import sleep

led = Pin("LED", Pin.OUT)
buzzer = Pin(10, Pin.OUT)


while True:
    buzzer.on()
    led.on()
    sleep(500)
    buzzer.off()
    led.off()