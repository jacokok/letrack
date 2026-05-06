# pyright: reportImplicitRelativeImport=false
try:
    import uasyncio as asyncio
except ImportError:
    import asyncio

import json

from mqtt_as import MQTTClient

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
        beam_stable_confirmation_ms,
        cross_lane_suppression_ms,
    ):
        self.beam = beam
        self.time = ticks_ms()
        event = Event(trackId)
        self.event = event
        self.prev_event = event
        self.trackId = trackId
        self.same_lane_cooldown_ms = same_lane_cooldown_ms
        self.beam_stable_confirmation_ms = beam_stable_confirmation_ms
        self.cross_lane_suppression_ms = cross_lane_suppression_ms
        self.last_event_ms = None
        self.pending_since_ms = None
        self.pending_hold_until_ms = None
        self.armed = True
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

        if not self.armed:
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
        self.pending_since_ms = now_ms
        self.pending_hold_until_ms = now_ms

        # If the other lane just triggered, give the signal a tiny moment to settle.
        if (
            last_any_trigger_ms is not None
            and last_any_track_id != self.trackId
            and ticks_diff(now_ms, last_any_trigger_ms) < self.cross_lane_suppression_ms
        ):
            self.pending_hold_until_ms = now_ms + self.cross_lane_suppression_ms

    def _clear_pending_trigger(self):
        self.pending_since_ms = None
        self.pending_hold_until_ms = None

    def _confirm_pending_trigger(self):
        if self.pending_since_ms is None:
            return

        if self.beam.value() != 0:
            self._clear_pending_trigger()
            return

        now_ms = ticks_ms()
        hold_until = self.pending_hold_until_ms
        if hold_until is not None and ticks_diff(now_ms, hold_until) < 0:
            return

        # Only count it if the beam has stayed broken long enough.
        elapsed = ticks_diff(now_ms, self.pending_since_ms)
        if elapsed < self.beam_stable_confirmation_ms:
            return

        self._clear_pending_trigger()
        self.armed = False
        self.last_event_ms = now_ms
        self.event = Event(self.trackId)

    async def check(self, mqtt: MQTTClient):
        if not self.armed and self.beam.value() != 0:
            self.armed = True

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
