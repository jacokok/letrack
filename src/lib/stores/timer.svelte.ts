export class Timer {
	#start = $state<Date | undefined>(undefined);

	#startTime = $derived(this.#start?.getTime() ?? 0);

	timer = $state<Date | undefined>(undefined);

	#seconds = $derived(this.timer?.getSeconds() ?? 0);
	#ms = $derived(this.timer?.getMilliseconds() ?? 0);
	#minutes = $derived(this.timer?.getMinutes() ?? 0);

	#lastLap = $state<Date | undefined>(undefined);
	isConfirmed = $state(false);

	get minutes() {
		return this.#minutes;
	}
	get minutesFmt() {
		return this.#minutes.toString().padStart(2, "0");
	}
	get ms() {
		return this.#ms;
	}

	get msFmt() {
		return this.#ms.toString().padStart(2, "0").substring(0, 2);
	}
	get seconds() {
		return this.#seconds;
	}
	get secondsFmt() {
		return this.#seconds.toString().padStart(2, "0");
	}

	get lastLap() {
		return `${this.#lastLap?.getMinutes().toString().padStart(2, "0") ?? "00"}:${this.#lastLap?.getSeconds().toString().padStart(2, "0") ?? "00"}:${this.#lastLap?.getMilliseconds().toString().padStart(2, "0") ?? "00"}`;
	}

	constructor() {
		$effect.root(() => {
			$effect(() => {
				if (this.#start == undefined) {
					return;
				}
				const interval = setInterval(() => {
					if (this.#start == undefined) {
						this.timer = undefined;
						return;
					}
					const current = new Date();
					const currentTime = current.getTime();

					this.timer = new Date(currentTime - this.#startTime);
				}, 10);

				return () => {
					clearInterval(interval);
				};
			});
		});
	}

	start = () => {
		this.#lastLap = new Date(this.timer?.getTime() ?? 0);
		this.#start = new Date();
	};

	stop = () => {
		this.#start = undefined;
	};
}
