WIFI_SSID = "letrack"
WIFI_PASSWD = "letrack123"
MQTT_SERVER = "192.168.0.100"
NTP_HOST = "192.168.0.100"
BEAM1 = 1
BEAM2 = 2
HOSTNAME = "letrack1"
# Do not allow another event on the same lane for this long.
SAME_LANE_COOLDOWN_MS = 4000
# Require the beam to stay broken for this long before counting it.
BEAM_STABLE_CONFIRMATION_MS = 10
# Very short delay to ignore cross-lane false triggers.
# Keep this small so two cars passing at nearly the same time can still count.
CROSS_LANE_SUPPRESSION_MS = 5
# Poll pending triggers frequently so short real beam breaks are still seen.
BEAM_POLL_INTERVAL_MS = 2
# Set to True to mute all buzzer sounds while testing.
MUTE_SPEAKER = False
