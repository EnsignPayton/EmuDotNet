using System;
using EmuDotNet.Core.Abstractions;

namespace EmuDotNet.Core
{
    public class SimpleMemory : IMemory
    {
        private const int DataSize = ushort.MaxValue + 1;

        private readonly byte[] _data = new byte[DataSize];

        public byte[] Data => _data;

        public SimpleMemory()
        {
        }

        public SimpleMemory(ReadOnlySpan<byte> data)
        {
            data.CopyTo(_data);
        }

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
