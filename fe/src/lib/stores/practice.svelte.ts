import { useLocalStorage } from "./useLocalStorage.svelte";

export const practice = useLocalStorage("practice", {
	track1: true,
	track2: false,
	track3: false,
	track4: false
});
