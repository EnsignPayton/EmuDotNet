using EmuDotNet.Core.Abstractions;

namespace EmuDotNet.Core.MC6800
{
    public class Processor : IProcessor
    {
        private readonly IMemory _memory;
        private readonly Registers _registers = new();

        // TODO: Remove later maybe
        public Registers Registers => _registers;

        public Processor(
            IMemory memory)
        {
            _memory = memory;
        }

        public void ExecuteClock()
        {
            Execute(NextInstruction());
        }

        private Instruction NextInstruction()
        {
            return (Instruction) NextImmediate();
        }

        private byte NextImmediate()
        {
            return _memory.GetByte(_registers.PC++);
        }

        private byte NextDirect()
        {
            return _memory.GetByte(NextImmediate());
        }

        private byte NextExtended()
        {
            var low = NextImmediate();
            var high = NextImmediate();
            var address = (high << 8) | low;
            return _memory.GetByte((ushort) address);
        }

        private byte NextIndexed()
        {
            var idx = NextImmediate();
            var address = _registers.IX + idx;
            return _memory.GetByte((ushort) address);
        }

        private byte NextRelative()
        {
            var rel = NextImmediate();
            var offset = unchecked((sbyte) rel);
            var address = _registers.PC + offset;
            return _memory.GetByte((ushort) address);
        }

        private byte Carry()
        {
            return _registers.C ? 1 : 0;
        }

        private void Execute(Instruction instruction)
        {
            switch (instruction)
            {
                case Instruction.ABA:
                    _registers.A += _registers.B;
                    break;
                case Instruction.ADC_A_IMM:
                    _registers.A += (byte) (NextImmediate() + Carry());
                    break;
                case Instruction.ADC_A_DIR:
                    _registers.A += (byte) (NextDirect() + Carry());
                    break;
                case Instruction.ADC_A_IDX:
                    _registers.A += (byte) (NextIndexed() + Carry());
                    break;
                case Instruction.ADC_A_EXT:
                    _registers.A += (byte) (NextExtended() + Carry());
                    break;
                case Instruction.ADC_B_IMM:
                    _registers.B += (byte) (NextImmediate() + Carry());
                    break;
                case Instruction.ADC_B_DIR:
                    _registers.B += (byte) (NextDirect() + Carry());
                    break;
                case Instruction.ADC_B_IDX:
                    _registers.B += (byte) (NextIndexed() + Carry());
                    break;
                case Instruction.ADC_B_EXT:
                    _registers.B += (byte) (NextExtended() + Carry());
                    break;
                case Instruction.ADD_A_IMM:
                    _registers.A += NextImmediate();
                    break;
                case Instruction.ADD_A_DIR:
                    _registers.A += NextDirect();
                    break;
                case Instruction.ADD_A_IDX:
                    _registers.A += NextIndexed();
                    break;
                case Instruction.ADD_A_EXT:
                    _registers.A += NextExtended();
                    break;
                case Instruction.ADD_B_IMM:
                    _registers.B += NextImmediate();
                    break;
                case Instruction.ADD_B_DIR:
                    _registers.B += NextDirect();
                    break;
                case Instruction.ADD_B_IDX:
                    _registers.B += NextIndexed();
                    break;
                case Instruction.ADD_B_EXT:
                    _registers.B += NextExtended();
                    break;
                default:
                    return;
            }
        }
    }
}
