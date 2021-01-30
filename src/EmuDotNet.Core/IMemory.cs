namespace EmuDotNet.Core
{
    public interface IMemory
    {
        byte GetByte(ushort address);
        void SetByte(ushort address, byte value);
    }
}
