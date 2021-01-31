namespace EmuDotNet.Core.MC6800
{
    public class ALU
    {
        private readonly Registers _registers;

        public ALU(Registers registers)
        {
            _registers = registers;
        }

        public byte Add(byte val1, byte val2, bool useCarry = false)
        {
            var carry = useCarry && _registers.C ? 1 : 0;

            var low1 = val1 & 0xF;
            var low2 = val2 & 0xF;
            var lowSum = low1 + low2 + carry;
            _registers.H = (lowSum & 0x10) != 0;

            var sum = val1 + val2 + carry;
            _registers.N = (sum & 0x80) != 0;
            _registers.Z = sum == 0;
            _registers.V = (val1 & 0x80) == 0 && (val2 & 0x80) == 0 && (sum & 0x80) != 0 ||
                           (val1 & 0x80) != 0 && (val2 & 0x80) != 0 && (sum & 0x80) == 0;
            _registers.C = (sum & 0x100) != 0;
            return (byte) sum;
        }

        public byte And(byte val1, byte val2)
        {
            var and = val1 & val2;
            _registers.N = (and & 0x80) != 0;
            _registers.Z = and == 0;
            return (byte) and;
        }

        public byte Clear()
        {
            _registers.N = false;
            _registers.Z = true;
            _registers.C = false;
            _registers.V = false;
            return 0;
        }

        public byte Complement(byte value)
        {
            var result = (byte) (value ^ 0xFF);
            _registers.N = (result & 0x80) != 0;
            _registers.Z = result == 0;
            _registers.V = false;
            _registers.C = true;
            return result;
        }

        public byte Negate(byte value)
        {
            var result = (byte) ((byte) (value ^ 0xFF) + 1);
            _registers.N = (result & 0x80) != 0;
            _registers.Z = result == 0;
            _registers.V = result == 0x80;
            _registers.C = result == 0;
            return result;
        }
    }
}
