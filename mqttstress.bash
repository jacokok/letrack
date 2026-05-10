#!/bin/bash
SECONDS=0

while (( SECONDS < 120 )); do
  echo "Running... elapsed time: $SECONDS seconds"
  mqttui publish "event" '{ "Id": "'$(uuidgen)'", "TrackId": '$((RANDOM % 4 + 1))', "Timestamp": "'$(date -u +"%Y-%m-%dT%H:%M:%S.%3NZ")'"}'
  sleep 0.05
done

echo "Loop finished after 2 minutes."
