using System;
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

        private byte NextIndexed()
        {
            var idx = NextImmediate();
            var address = _registers.IX + idx;
            return _memory.GetByte((ushort)address);
        }

        private byte NextExtended()
        {
            var low = NextImmediate();
            var high = NextImmediate();
            var address = (high << 8) | low;
            return _memory.GetByte((ushort) address);
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

        private byte Carry()
        {
            return _registers.C ? 1 : 0;
        }

        private void OperateOn(Accumulator reg, Func<byte, byte> func)
        {
            if (reg == Accumulator.A)
                _registers.A = func(_registers.A);
            else
                _registers.B = func(_registers.B);
        }

        private void OperateOn(Accumulator reg, Func<byte, int> func)
        {
            OperateOn(reg, x => (byte) func(x));
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
                default:
                    return;
            }
        }

        private byte AdcAdd(byte val1, byte val2)
        {
            var low1 = val1 & 0xF;
            var low2 = val2 & 0xF;
            var lowSum = low1 + low2;
            _registers.H = (lowSum & 0x10) != 0;

            var sum = val1 + val2;
            _registers.N = (sum & 0x80) != 0;
            _registers.Z = sum == 0;
            _registers.V = (val1 & 0x80) == 0 && (val2 & 0x80) == 0 && (sum & 0x80) != 0 ||
                           (val1 & 0x80) != 0 && (val2 & 0x80) != 0 && (sum & 0x80) == 0;
            _registers.C = (sum & 0x100) != 0;
            return (byte) sum;
        }

        private void Add(Accumulator reg, AddressingMode mode)
        {
            OperateOn(reg, x => AdcAdd(x, NextValue(mode)));
        }

        private void AddAccumulators()
        {
            _registers.A = AdcAdd(_registers.A, _registers.B);
        }

        private void AddWithCarry(Accumulator reg, AddressingMode mode)
        {
            OperateOn(reg, x => x + NextValue(mode) + Carry());
        }
    }
}
