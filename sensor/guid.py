import os

import ubinascii


class GUID:
    def __init__(self, raw_bytes: bytes):
        if len(raw_bytes) != 16:
            raise ValueError("bytes arg must be 16 bytes long")
        self._bytes: bytes = raw_bytes

    @property
    def hex(self) -> str:
        return ubinascii.hexlify(self._bytes).decode()

    def __str__(self) -> str:  # pyright: ignore[reportImplicitOverride]
        h = self.hex
        return "-".join((h[0:8], h[8:12], h[12:16], h[16:20], h[20:32]))

    def __repr__(self) -> str:  # pyright: ignore[reportImplicitOverride]
        return "<UUID: %s>" % str(self)


def new() -> GUID:
    """Generates a random UUID compliant to RFC 4122 pg.14"""
    random = bytearray(os.urandom(16))
    random[6] = (random[6] & 0x0F) | 0x40
    random[8] = (random[8] & 0x3F) | 0x80
    return GUID(raw_bytes=bytes(random))
