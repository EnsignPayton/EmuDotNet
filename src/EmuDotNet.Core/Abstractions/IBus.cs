namespace EmuDotNet.Core.Abstractions;

public interface IBus
{
    byte GetByte(ushort address);
    void SetByte(ushort address, byte value);

    public byte this[ushort address]
    {
        get => GetByte(address);
        set => SetByte(address, value);
    }
}
