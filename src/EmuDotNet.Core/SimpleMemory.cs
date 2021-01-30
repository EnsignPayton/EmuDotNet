namespace EmuDotNet.Core
{
    public class SimpleMemory : IMemory
    {
        private readonly byte[] _data = new byte[ushort.MaxValue + 1];

        public byte GetByte(ushort address)
        {
            return _data[address];
        }

        public void SetByte(ushort address, byte value)
        {
            _data[address] = value;
        }
    }
}
