import { useLocalStorage } from "./useLocalStorage.svelte";

export const practice = useLocalStorage("practice", { numberOfTracks: 1 });
