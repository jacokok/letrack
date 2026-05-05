# pyright: reportImplicitRelativeImport=false

from time import gmtime

from guid import GUID, new


class Event:
    def __init__(self, trackId: int):
        self.trackId: int = trackId
        self.id: GUID = new()
        self.timestamp: tuple[int, ...] = gmtime()

    def __str__(self) -> str:  # pyright: ignore[reportImplicitOverride]
        return f"{self.id.hex} {self.timestamp}"
