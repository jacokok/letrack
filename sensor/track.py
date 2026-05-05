# pyright: reportImplicitRelativeImport=false
try:
    import uasyncio as asyncio
except ImportError:
    import asyncio

import json

try:
    from .buzzer import Buzzer
    from .event import Event
except ImportError:
    from buzzer import Buzzer
    from event import Event

from machine import Pin
from utime import gmtime, ticks_diff, ticks_ms, time_ns

DEBOUNCE_MS = 500


class BreakBeam:
    last_any_trigger_ms = None
    last_any_track_id = None

    def __init__(
        self,
        beam,
        trackId,
        same_lane_cooldown_ms,
        cross_lane_suppression_ms,
    ):
        self.beam = beam
        self.time = ticks_ms()
        event = Event(trackId)
        self.event = event
        self.prev_event = event
        self.trackId = trackId
        self.same_lane_cooldown_ms = same_lane_cooldown_ms
        self.cross_lane_suppression_ms = cross_lane_suppression_ms
        self.last_event_ms = None
        self.pending_trigger_ms = None
        self.led = Pin("LED", Pin.OUT)
        self.buzzer = Buzzer()
        self._irq = self.beam.irq(
            handler=self.break_handler,
            trigger=Pin.IRQ_FALLING,
        )

    def break_handler(self, _pin):
        now_ms = ticks_ms()
        debounce_elapsed = ticks_diff(now_ms, self.time)
        if debounce_elapsed <= DEBOUNCE_MS:
            return

        last_event_ms = self.last_event_ms
        if last_event_ms is not None:
            cooldown_elapsed = ticks_diff(now_ms, last_event_ms)
            if cooldown_elapsed < self.same_lane_cooldown_ms:
                return

        last_any_trigger_ms = BreakBeam.last_any_trigger_ms
        last_any_track_id = BreakBeam.last_any_track_id
        BreakBeam.last_any_trigger_ms = now_ms
        BreakBeam.last_any_track_id = self.trackId

        self.time = now_ms
        if (
            last_any_trigger_ms is not None
            and last_any_track_id != self.trackId
            and ticks_diff(now_ms, last_any_trigger_ms) < self.cross_lane_suppression_ms
        ):
            self.pending_trigger_ms = now_ms
            return

        self.last_event_ms = now_ms
        self.event = Event(self.trackId)

    def _confirm_pending_trigger(self):
        if self.pending_trigger_ms is None:
            return

        now_ms = ticks_ms()
        elapsed = ticks_diff(now_ms, self.pending_trigger_ms)
        if elapsed < self.cross_lane_suppression_ms:
            return

        self.pending_trigger_ms = None

        # Pull-up input means 0 while the beam is actually broken.
        if self.beam.value() != 0:
            return

        self.last_event_ms = now_ms
        self.event = Event(self.trackId)

    async def check(self, mqtt):
        self._confirm_pending_trigger()
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
