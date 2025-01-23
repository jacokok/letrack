from machine import Pin

light = Pin(6, Pin.OUT)
light.on()
print("boot")
