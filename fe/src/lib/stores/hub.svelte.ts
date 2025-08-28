import {
	HubConnection,
	HubConnectionBuilder,
	LogLevel,
	HubConnectionState
} from "@microsoft/signalr";

import { PUBLIC_API_URL } from "$env/static/public";

class Hub {
	connection: HubConnection | undefined = $state(undefined);
	state: HubConnectionState = $state(HubConnectionState.Disconnected);

	public async init() {
		this.connection = new HubConnectionBuilder()
			.withUrl(`${PUBLIC_API_URL}/hub`)
			.withAutomaticReconnect()
			.configureLogging(LogLevel.None)
			.build();

		const onStateUpdatedCallback = () => {
			this.state = this.connection?.state ?? HubConnectionState.Disconnected;
		};

		this.connection.onclose(onStateUpdatedCallback);
		this.connection.onreconnected(onStateUpdatedCallback);
		this.connection.onreconnecting(onStateUpdatedCallback);

		try {
			await this.connection.start();
			onStateUpdatedCallback();
		} catch (err) {
			onStateUpdatedCallback();
			console.error(err);
		}
	}

	public on(methodName: string, method: (...args: any[]) => void) {
		if (!this.connection) {
			return;
		}
		this.connection.on(methodName, method);
	}

	public off(methodName: string, method: (...args: any[]) => void) {
		if (!this.connection) {
			return;
		}
		this.connection.off(methodName, method);
	}

	public disconnect() {
		this.connection?.stop();
		this.state = this.connection?.state ?? HubConnectionState.Disconnected;
	}
}

export const hub = new Hub();
