try:
    import uasyncio as asyncio
except ImportError:
    import asyncio

import json

from buzzer import Buzzer
from event import Event
from machine import Pin
from utime import gmtime, ticks_diff, ticks_ms, time_ns


class BreakBeam:
    def __init__(self, beam, trackId):
        self.beam = beam
        self.time = ticks_ms()
        event = Event(trackId)
        self.event = event
        self.prev_event = event
        self.trackId = trackId
        self.led = Pin("LED", Pin.OUT)
        self.buzzer = Buzzer()
        self._irq = self.beam.irq(
            handler=self.break_handler,
            trigger=Pin.IRQ_FALLING,
        )

    def break_handler(self, _pin):
        diff = ticks_diff(ticks_ms(), self.time)
        if diff > 500:
            self.time = ticks_ms()
            self.event = Event(self.trackId)

    async def check(self, mqtt):
        if self.event.id != self.prev_event.id:
            self.prev_event = self.event
            asyncio.create_task(self.publish_event(self.event, mqtt))

    async def publish_event(self, event, mqtt):
        current_ns = time_ns()
        utc_string = utc_from_ns(current_ns)
        event_json = json.dumps(
            {
                "Id": str(event.id),
                "TrackId": event.trackId,
                "Timestamp": utc_string,
            }
        )
        await mqtt.publish("event", event_json)
        self.buzzer.lap()


def utc_from_ns(ns):
    try:
        seconds = ns // 1_000_000_000
        nanoseconds = ns % 1_000_000_000
        tm = gmtime(seconds)

        return "{:04d}-{:02d}-{:02d}T{:02d}:{:02d}:{:02d}.{:06d}Z".format(
            tm[0],
            tm[1],
            tm[2],
            tm[3],
            tm[4],
            tm[5],
            nanoseconds // 1000,
        )
    except OverflowError:
        print("OverflowError: Nanoseconds value is too large.")
        return None
    except Exception as e:
        print(f"An error occurred: {e}")
        return None
