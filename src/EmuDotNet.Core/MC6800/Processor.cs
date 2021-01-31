using System;
using EmuDotNet.Core.Abstractions;

namespace EmuDotNet.Core.MC6800
{
    public class Processor : IProcessor
    {
        private readonly IMemory _memory;
        private readonly Registers _registers = new();

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

        private ushort DirectAddress()
        {
            return NextImmediate();
        }

        private byte NextDirect()
        {
            return _memory.GetByte(NextImmediate());
        }

        private ushort IndexedAddress()
        {
            var idx = NextImmediate();
            var address = _registers.IX + idx;
            return (ushort) address;
        }

        private byte NextIndexed()
        {
            return _memory.GetByte(IndexedAddress());
        }

        private ushort ExtendedAddress()
        {
            var high = NextImmediate();
            var low = NextImmediate();
            var address = (high << 8) | low;
            return (ushort) address;
        }

        private byte NextExtended()
        {
            return _memory.GetByte(ExtendedAddress());
        }

        private byte NextRelative()
        {
            var rel = NextImmediate();
            var offset = unchecked((sbyte) rel);
            var address = _registers.PC + offset;
            return _memory.GetByte((ushort) address);
        }

        private byte NextValue(AddressingMode mode) => mode switch
        {
            AddressingMode.IMM => NextImmediate(),
            AddressingMode.DIR => NextDirect(),
            AddressingMode.IDX => NextIndexed(),
            AddressingMode.EXT => NextExtended(),
            AddressingMode.REL => NextRelative(),
            _ => throw new ArgumentException($"Addressing mode {mode} does not read from memory", nameof(mode))
        };

        private ushort NextAddress(AddressingMode mode) => mode switch
        {
            AddressingMode.DIR => DirectAddress(),
            AddressingMode.IDX => IndexedAddress(),
            AddressingMode.EXT => ExtendedAddress(),
            _ => throw new ArgumentException($"Addressing mode {mode} does not read from memory", nameof(mode))
        };

        private void OperateOn(Accumulator reg, Func<byte, byte> func)
        {
            if (reg == Accumulator.A)
                _registers.A = func(_registers.A);
            else
                _registers.B = func(_registers.B);
        }

        private byte GetAccumulator(Accumulator reg)
        {
            return reg == Accumulator.A ? _registers.A : _registers.B;
        }

        private void Execute(Instruction instruction)
        {
            switch (instruction)
            {
                case Instruction.ADD_A_IMM:
                case Instruction.ADD_A_DIR:
                case Instruction.ADD_A_IDX:
                case Instruction.ADD_A_EXT:
                case Instruction.ADD_B_IMM:
                case Instruction.ADD_B_DIR:
                case Instruction.ADD_B_IDX:
                case Instruction.ADD_B_EXT:
                    Add(instruction.GetAccumulator(), instruction.GetMode());
                    break;
                case Instruction.ABA:
                    AddAccumulators();
                    break;
                case Instruction.ADC_A_IMM:
                case Instruction.ADC_A_DIR:
                case Instruction.ADC_A_IDX:
                case Instruction.ADC_A_EXT:
                case Instruction.ADC_B_IMM:
                case Instruction.ADC_B_DIR:
                case Instruction.ADC_B_IDX:
                case Instruction.ADC_B_EXT:
                    AddWithCarry(instruction.GetAccumulator(), instruction.GetMode());
                    break;
                case Instruction.AND_A_IMM:
                case Instruction.AND_A_DIR:
                case Instruction.AND_A_IDX:
                case Instruction.AND_A_EXT:
                case Instruction.AND_B_IMM:
                case Instruction.AND_B_DIR:
                case Instruction.AND_B_IDX:
                case Instruction.AND_B_EXT:
                    And(instruction.GetAccumulator(), instruction.GetMode());
                    break;
                case Instruction.BIT_A_IMM:
                case Instruction.BIT_A_DIR:
                case Instruction.BIT_A_IDX:
                case Instruction.BIT_A_EXT:
                case Instruction.BIT_B_IMM:
                case Instruction.BIT_B_DIR:
                case Instruction.BIT_B_IDX:
                case Instruction.BIT_B_EXT:
                    Bit(instruction.GetAccumulator(), instruction.GetMode());
                    break;
                case Instruction.CLR_A:
                case Instruction.CLR_B:
                    ClearAccumulator(instruction.GetAccumulator());
                    break;
                case Instruction.CLR_IDX:
                case Instruction.CLR_EXT:
                    ClearMemory(instruction.GetMode());
                    break;
                // TODO: Come back to cmp, it's a flag set sub
                case Instruction.COM_A:
                case Instruction.COM_B:
                    ComplementAccumulator(instruction.GetAccumulator());
                    break;
                case Instruction.COM_IDX:
                case Instruction.COM_EXT:
                    ComplementMemory(instruction.GetMode());
                    break;
                case Instruction.NEG_A:
                case Instruction.NEG_B:
                    NegateAccumulator(instruction.GetAccumulator());
                    break;
                case Instruction.NEG_IDX:
                case Instruction.NEG_EXT:
                    NegateMemory(instruction.GetMode());
                    break;
                default:
                    return;
            }
        }

        private void Add(Accumulator reg, AddressingMode mode)
        {
            OperateOn(reg, x => AdcAdd(x, NextValue(mode)));
        }

        private void AddAccumulators()
        {
            OperateOn(Accumulator.A, x => AdcAdd(x, _registers.B));
        }

        private void AddWithCarry(Accumulator reg, AddressingMode mode)
        {
            OperateOn(reg, x => AdcAdd(x, NextValue(mode), true));
        }

        private byte AdcAdd(byte val1, byte val2, bool useCarry = false)
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
            return (byte)sum;
        }

        private void And(Accumulator reg, AddressingMode mode)
        {
            OperateOn(reg, x => AdcAnd(x, NextValue(mode)));
        }

        private byte AdcAnd(byte val1, byte val2)
        {
            var and = val1 & val2;
            _registers.N = (and & 0x80) != 0;
            _registers.Z = and == 0;
            return (byte) and;
        }

        private void Bit(Accumulator reg, AddressingMode mode)
        {
            AdcAnd(GetAccumulator(reg), NextValue(mode));
        }

        private void ClearAccumulator(Accumulator reg)
        {
            _registers.N = false;
            _registers.Z = true;
            _registers.C = false;
            _registers.V = false;
            OperateOn(reg, _ => 0);
        }

        private void ClearMemory(AddressingMode mode)
        {
            _registers.N = false;
            _registers.Z = true;
            _registers.C = false;
            _registers.V = false;
            var address = NextAddress(mode);
            _memory.SetByte(address, 0);
        }

        private void ComplementAccumulator(Accumulator reg)
        {
            OperateOn(reg, x =>
            {
                var result = (byte) (x ^ 0xFF);
                _registers.N = (result & 0x80) != 0;
                _registers.Z = result == 0;
                _registers.V = false;
                _registers.C = true;
                return result;
            });
        }

        private void ComplementMemory(AddressingMode mode)
        {
            var address = NextAddress(mode);
            var value = _memory.GetByte(address);
            var result = (byte) (value ^ 0xFF);
            _registers.N = (result & 0x80) != 0;
            _registers.Z = result == 0;
            _registers.V = false;
            _registers.C = true;
            _memory.SetByte(address, result);
        }

        private void NegateAccumulator(Accumulator reg)
        {
            OperateOn(reg, x =>
            {
                var result = (byte) ((byte) (x ^ 0xFF) + 1);
                _registers.N = (result & 0x80) != 0;
                _registers.Z = result == 0;
                _registers.V = result == 0x80;
                _registers.C = result == 0;
                return result;
            });
        }

        private void NegateMemory(AddressingMode mode)
        {
            var address = NextAddress(mode);
            var value = _memory.GetByte(address);
            var result = (byte) ((byte) (value ^ 0xFF) + 1);
            _registers.N = (result & 0x80) != 0;
            _registers.Z = result == 0;
            _registers.V = result == 0x80;
            _registers.C = result == 0;
            _memory.SetByte(address, result);
        }
    }
}
