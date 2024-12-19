const timeSpanToParts = (timeSpan?: string) => {
	if (timeSpan == null) {
		timeSpan = "00:00:00.000";
	}

	const isMinus = timeSpan.startsWith("-");

	const [hoursRaw, minutesRaw, secondsAndMs] = timeSpan.split(":");

	const hours = parseInt(hoursRaw);
	const hoursFmt = hours.toString().padStart(2, "0");

	const minutes = parseInt(minutesRaw);
	const minutesFmt = minutes.toString().padStart(2, "0");

	const [secondsRaw, ms] = secondsAndMs.split(".");

	const seconds = parseInt(secondsRaw);
	const secondsFmt = seconds.toString().padStart(2, "0");

	const msFmt = ms.toString().padStart(2, "0").substring(0, 3);

	const value = `${minutesFmt}:${secondsFmt}:${msFmt}`;
	const toSeconds = hours * 60 * 60 + minutes * 60 + seconds + Number(ms) / 10000000;

	return {
		hours,
		hoursFmt,
		minutes,
		minutesFmt,
		seconds,
		secondsFmt,
		ms,
		msFmt,
		value,
		toSeconds,
		isMinus
	};
};

export { timeSpanToParts };
