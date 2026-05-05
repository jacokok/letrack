# WIFI_SSID = "wifissd"
# WIFI_PASSWD = "wifipassword"
# MQTT_SERVER = "10.0.0.2"
# NTP_HOST = "time.google.com"  # or ip of ntp server if no internet on network. Example in docker-compose.yml
# BEAM1 = 1
# BEAM2 = 2

WIFI_SSID = "letrack"
WIFI_PASSWD = "letrack123"
MQTT_SERVER = "192.168.0.100"
NTP_HOST = "192.168.0.100"
BEAM1 = 1
BEAM2 = 2
HOSTNAME = "letrack1"

# Do not allow another event on the same lane for this long.
SAME_LANE_COOLDOWN_MS = 5000

# Short block to suppress false cross-triggering between lanes.
# Keep this small so two cars passing at nearly the same time can still count.
CROSS_LANE_SUPPRESSION_MS = 20

# Set to True to mute all buzzer sounds while testing.
MUTE_SPEAKER = False

# WIFI_SSID = "letrack"
# WIFI_PASSWD = "letrack123"
# MQTT_SERVER = "192.168.0.100"
# NTP_HOST = "192.168.0.100"
# BEAM1 = 3
# BEAM2 = 4
# HOSTNAME = "letrack2"
