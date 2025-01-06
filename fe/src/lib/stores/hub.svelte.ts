import {
	HubConnection,
	HubConnectionBuilder,
	LogLevel,
	HubConnectionState
} from "@microsoft/signalr";

import { PUBLIC_API_URL } from "$env/static/public";

const createHub = () => {
	let connection: HubConnection;
	let hubConnectionState = $state(HubConnectionState.Disconnected);

	const onStateUpdatedCallback = () => {
		hubConnectionState = connection?.state ?? HubConnectionState.Disconnected;
	};

	const init = async () => {
		connection = new HubConnectionBuilder()
			.withUrl(`${PUBLIC_API_URL}/hub`)
			.withAutomaticReconnect({
				nextRetryDelayInMilliseconds: (retryContext) => {
					if (retryContext.elapsedMilliseconds < 120000) {
						return 1000;
					} else {
						return 15000;
					}
				}
			})
			.configureLogging(LogLevel.None)
			.build();

		connection
			.start()
			.then(onStateUpdatedCallback)
			.catch((err) => {
				console.error(err);
			});
		onStateUpdatedCallback();
	};

	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	const on = (methodName: string, method: (...args: any[]) => void) => {
		if (!connection) {
			return;
		}
		connection.on(methodName, method);
	};

	// eslint-disable-next-line @typescript-eslint/no-explicit-any
	const off = (methodName: string, method: (...args: any[]) => void) => {
		if (!connection) {
			return;
		}
		connection.off(methodName, method);
	};

	const disconnect = () => {
		connection?.stop();
	};

	$effect.root(() => {
		if (!connection) {
			hubConnectionState = HubConnectionState.Disconnected;
			return;
		}

		if (connection.state !== hubConnectionState) {
			hubConnectionState = connection.state;
		}

		connection.onclose(onStateUpdatedCallback);
		connection.onreconnected(onStateUpdatedCallback);
		connection.onreconnecting(onStateUpdatedCallback);

		$effect(() => {
			if (connection?.state === HubConnectionState.Disconnected) {
				connection
					.start()
					.then(onStateUpdatedCallback)
					.catch((err) => {
						console.error(err);
					});
				onStateUpdatedCallback();

				return () => {
					connection?.stop();
				};
			}
		});

		return () => {
			connection?.stop();
		};
	});

	return {
		get state() {
			return hubConnectionState;
		},
		get connection() {
			return connection;
		},
		init,
		on,
		off,
		disconnect
	};
};

export const hub = createHub();
