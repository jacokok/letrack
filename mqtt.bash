#!/usr/bin/env bash
set -euo pipefail

# Usage:
#   ./stress-mqtt.sh [TOTAL=10000] [DURATION_SEC=60] [WORKERS=10] [TOPIC=event]
#
# Example:
#   ./stress-mqtt.sh 10000 60 10 event

TOTAL="${1:-1000}"
DURATION="${2:-60}"
WORKERS="${3:-10}"
TOPIC="${4:-event}"

# sanity
if ! command -v mqttui >/dev/null 2>&1; then
  echo "mqttui not found in PATH. Install or add it to PATH and re-run."
  exit 2
fi

if ! command -v uuidgen >/dev/null 2>&1; then
  echo "uuidgen not found in PATH. Install 'uuid-runtime' (Debian/Ubuntu) or ensure uuidgen is available."
  exit 2
fi

# compute per-worker and per-worker delay (float)
# per worker message count = base + distribute remainder
per_worker=$((TOTAL / WORKERS))
remainder=$((TOTAL % WORKERS))

# per-worker delay D = DURATION * WORKERS / TOTAL
# use awk for float math
delay=$(awk -v d="$DURATION" -v w="$WORKERS" -v t="$TOTAL" 'BEGIN{printf "%.6f", (d * w) / t}')

echo "Stress test:"
echo "  total messages: $TOTAL"
echo "  duration (s):   $DURATION"
echo "  workers:        $WORKERS"
echo "  topic:          $TOPIC"
echo "  per-worker:     $per_worker (+some get +1 for remainder)"
echo "  per-message delay per worker: ${delay}s (approx)"
echo

# temp dir for counters
tmpdir=$(mktemp -d)
trap 'echo; echo "Interrupted - killing workers..."; pkill -P $$ || true; sleep 1; ls -1 "$tmpdir"/*.count 2>/dev/null || true; rm -rf "$tmpdir"; exit 1' INT TERM

worker() {
  local id="$1"
  local count="$2"
  local delay="$3"
  local counterfile="$4"

  local i=0
  for ((i = 1; i <= count; i++)); do
    # payload as in README (use random TrackId 1..2)
    mqttui publish "$TOPIC" '{ "Id": "'$(uuidgen)'", "TrackId": '$((RANDOM % 4 + 1))', "Timestamp": "'$(date -u +"%Y-%m-%dT%H:%M:%S.%3NZ")'"}' >/dev/null 2>&1 || true

    # increment local counter (fast)
    printf "%d\n" "$i" > "$counterfile"

    # add a tiny jitter to avoid perfectly synchronized bursts
    if [[ "$delay" == "0" || "$delay" == "0.000000" ]]; then
      # no sleep
      :
    else
      # jitter up to ±10% of delay
      jitter=$(awk -v d="$delay" 'BEGIN{srand(); printf "%f", (d * (0.9 + rand() * 0.2))}')
      # use sleep with fractional seconds
      sleep "$jitter"
    fi
  done
}

pids=()
# spawn workers
for ((w = 1; w <= WORKERS; w++)); do
  extra=0
  if (( w <= remainder )); then
    extra=1
  fi
  worker_count=$((per_worker + extra))
  counterfile="$tmpdir/worker_${w}.count"
  printf "0\n" > "$counterfile"
  worker "$w" "$worker_count" "$delay" "$counterfile" &
  pids+=("$!")
  printf "Started worker %d (pid %s) count=%d\n" "$w" "${pids[-1]}" "$worker_count"
done

# show progress every 2s
SECONDS=0
while true; do
  sleep 2
  total_sent=0
  for f in "$tmpdir"/*.count; do
    if [[ -f "$f" ]]; then
      cnt=$(cat "$f" 2>/dev/null || echo 0)
      total_sent=$((total_sent + cnt))
    fi
  done
  printf "\rElapsed: %02ds - Sent: %d / %d" "$SECONDS" "$total_sent" "$TOTAL"
  # all done?
  if (( total_sent >= TOTAL )); then
    break
  fi
  # ensure we don't loop forever if workers died
  all_dead=true
  for pid in "${pids[@]}"; do
    if kill -0 "$pid" 2>/dev/null; then
      all_dead=false
      break
    fi
  done
  if $all_dead; then
    echo
    echo "All workers have exited early. Sent $total_sent messages."
    break
  fi
done

# wait for workers to exit
wait

echo
total_sent=0
for f in "$tmpdir"/*.count; do
  cnt=$(cat "$f" 2>/dev/null || echo 0)
  total_sent=$((total_sent + cnt))
done

echo "Done. total sent: $total_sent (target $TOTAL)"
rm -rf "$tmpdir"
exit 0