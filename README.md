# LeTrack Sensor

This is the hardware part for LeTrack. This includes micropython code to detect lap events.

![model](./assets/screenshot.png)

This project consist of 3 parts and 3 separate repositories.

- <https://github.com/jacokok/letrack-api> (Back End)
- <https://github.com/jacokok/letrack> (Front End)
- <https://github.com/jacokok/letrack-sensor> (Hardware)

## Hardware

- IR break beam sensors
- Active Buzzer
- Raspberry Pi Pico W

## 3D Model Bridge

This consist of two parts. The front and the back. It has slots for all required components to go in.

- ![Back](./assets/LeTrack-Back.stl)
- ![Front](./assets/LeTrack-Front.stl)

### 3D Model Additional Parts

- 6x M2 screws to fit the raspberry pi and the speaker.
- 4x M3 Screws for the corners

## DEV Setup

1. Install micropython on raspberry pi
2. Setup virtual environment for python and install requirements
3. Use rshell to do the rest
4. Install packages using mip
5. Create config.py based on example-config.py

## Setup Virtual Environment

```bash
# Setup venv
python3 -m venv venv
source venv/bin/activate
# Install requirements
pip install -r requirements.txt
```

## shell

```bash
rshell
rshell ls
rshell
# ctrl-x seems to close repl
ls /pyboard

rshell repl
# sync src with pyboard
rshell rsync src /pyboard

# run specific file in repl
exec(open('file.py').read())
```

## Install Packages

```bash
# Install mip packages
rshell
repl
import mip
mip.install('umqtt.simple')
```
