using System.Runtime.InteropServices;

namespace EmuDotNet.Core;

public static class ByteUtils
{
    public static byte ToByte(this sbyte value) =>
        MemoryMarshal.Cast<sbyte, byte>(stackalloc sbyte[] {value})[0];

    public static sbyte ToSByte(this byte value) =>
        MemoryMarshal.Cast<byte, sbyte>(stackalloc byte[] {value})[0];
}
