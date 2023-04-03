# Message Encoding Format

This document describes the message encoding format for the communication protocol used between the client and server applications.

## Overview

The messages sent between the client and server applications are encoded in a binary format. Each message consists of a series of bytes, where the first byte indicates the type of action to be performed, followed by the data associated with the action.

## Action Types

The following action types are supported:

- Register: This action is used by the client to register a new user with the server. The associated data for this action includes the user's name.

## Message Format

### Register Action

The Register action is identified by the first byte in the message being set to 0x01. The remaining bytes in the message are used to encode the user's name. The format of the message is as follows:

| Byte | Description             |
| ---- | ----------------------- |
| 0    | Action Type (0x01)      |
| 1-4  | Length of Name (uint32) |
| 5-n  | Name (ASCII String)     |

The first byte of the message indicates that this is a Register action. The next 4 bytes represent the length of the user's name as a 32-bit unsigned integer in network byte order. The remaining bytes encode the user's name as an ASCII string.

## Example

To register a new user with the name "John Doe", the following message would be sent from the client to the server:

```
01 00 00 00 08 4A 6F 68 6E 20 44 6F 65
```

In this message, the first byte indicates that this is a Register action. The next 4 bytes (00 00 00 08) represent the length of the name as 8, and the remaining bytes (4A 6F 68 6E 20 44 6F 65) encode the ASCII string "John Doe".

## Conclusion

This concludes the description of the message encoding format for the communication protocol used between the client and server applications.
