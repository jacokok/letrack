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

		this.connection.onclose(this.onStateUpdatedCallback);
		this.connection.onreconnected(this.onStateUpdatedCallback);
		this.connection.onreconnecting(this.onStateUpdatedCallback);

		try {
			await this.connection.start();
			this.onStateUpdatedCallback();
		} catch (err) {
			this.onStateUpdatedCallback();
			console.error(err);
		}
	}

	private onStateUpdatedCallback() {
		this.state = this.connection?.state ?? HubConnectionState.Disconnected;
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
		this.onStateUpdatedCallback();
	}
}
export const hub = new Hub();
