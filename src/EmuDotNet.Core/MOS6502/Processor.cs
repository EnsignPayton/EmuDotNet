using System.Runtime.InteropServices;
using EmuDotNet.Core.Abstractions;

namespace EmuDotNet.Core.MOS6502;

public class Processor : IProcessor
{
    private readonly Registers _reg = new();
    private readonly ALU _alu;
    private readonly IBus _bus;
    private int _cycles;

    public Registers Registers => _reg;
    public IBus Bus => _bus;
    public bool IsExecuting => _cycles > 0;

    public Processor(IBus bus)
    {
        _alu = new ALU(_reg);
        _bus = bus;
        Reset();
    }

    public void ExecuteClock()
    {
        if (!IsExecuting)
        {
            var val = _bus[_reg.PC];
            _reg.PC++;
            _cycles++;

            var (instruction, mode) = OpcodeParser.Parse(val);
            var operand = ReadOperand(mode);
            Execute(instruction, operand);
        }

        _cycles--;
    }

    public void Reset()
    {
        _reg.I = true;
        // TODO: Decrement SP?
    }

    public void Interrupt()
    {
        throw new NotImplementedException();
    }

    public void InterruptNonMaskable()
    {
        throw new NotImplementedException();
    }

    private byte ReadOperand(AddressMode mode)
    {
        switch (mode)
        {
            case AddressMode.IMP:
            {
                _cycles++;
                return _reg.A;
            }
            case AddressMode.IMM:
            {
                var val = _bus[_reg.PC];
                _reg.PC++;
                _cycles++;
                return val;
            }
            case AddressMode.ZPG:
            {
                var address = _bus[_reg.PC];
                _reg.PC++;
                _cycles++;
                var val = _bus[address];
                _cycles++;
                return val;
            }
            case AddressMode.ZPX:
            {
                var addressBase = _bus[_reg.PC];
                _reg.PC++;
                _cycles++;
                var offset = _reg.X;
                var address = (byte)((addressBase + offset) & 0xFF);
                _cycles++;
                var val = _bus[address];
                _cycles++;
                return val;
            }
            case AddressMode.ABS:
            {
                var low = _bus[_reg.PC];
                _reg.PC++;
                _cycles++;
                var high = _bus[_reg.PC];
                _reg.PC++;
                _cycles++;
                var address = (ushort) ((high << 8) + low);
                var val = _bus[address];
                _cycles++;
                return val;
            }
            case AddressMode.ABX:
            {
                var low = _bus[_reg.PC];
                _reg.PC++;
                _cycles++;
                var high = _bus[_reg.PC];
                _reg.PC++;
                _cycles++;
                var addressBase = (ushort) ((high << 8) + low);
                var address = (ushort) (addressBase + _reg.X);
                if (low + _reg.X > 256) _cycles++;
                var val = _bus[address];
                _cycles++;
                return val;
            }
            case AddressMode.ABY:
            {
                var low = _bus[_reg.PC];
                _reg.PC++;
                _cycles++;
                var high = _bus[_reg.PC];
                _reg.PC++;
                _cycles++;
                var addressBase = (ushort) ((high << 8) + low);
                var address = (ushort) (addressBase + _reg.Y);
                if (low + _reg.Y > 256) _cycles++;
                var val = _bus[address];
                _cycles++;
                return val;
            }
            case AddressMode.INX:
            {
                var zAddressBase = _bus[_reg.PC];
                _reg.PC++;
                _cycles++;
                var zAddress = (ushort) ((zAddressBase + _reg.X) & 0xFF);
                var low = _bus[zAddress];
                _cycles++;
                var high = _bus[(ushort) (zAddress + 1)];
                _cycles++;
                var address = (ushort) ((high << 8) + low);
                _cycles++;
                var val = _bus[address];
                _cycles++;
                return val;
            }
            case AddressMode.INY:
            {
                var zAddress = _bus[_reg.PC];
                _reg.PC++;
                _cycles++;
                var addressBaseLow = _bus[zAddress];
                _cycles++;
                var addressBaseHigh = _bus[(ushort) (zAddress + 1)];
                _cycles++;
                var addressBase = (ushort) ((addressBaseHigh << 8) + addressBaseLow);
                var address = (ushort) (addressBase + _reg.Y);
                if ((addressBaseLow + _reg.Y) > 256) _cycles++;
                var val = _bus[address];
                _cycles++;
                return val;
            }
            case AddressMode.REL:
            {
                var offset = _bus[_reg.PC];
                _reg.PC++;
                _cycles++;
                return offset;
            }
            default:
                throw new NotImplementedException();
        }
    }

    private void Execute(Instruction instruction, byte operand)
    {
        switch (instruction)
        {
            case Instruction.ADC:
                _alu.AddWithCarry(operand);
                break;
            case Instruction.AND:
                _alu.LogicalAnd(operand);
                break;
            case Instruction.ASL:
                _reg.A = _alu.ArithmeticShiftLeft(_reg.A);
                break;
            case Instruction.BCC:
                BranchIfCarryClear(operand);
                break;
            case Instruction.BCS:
                BranchIfCarrySet(operand);
                break;
            case Instruction.BEQ:
                BranchIfEqual(operand);
                break;
            case Instruction.BIT:
                _alu.BitTest(operand);
                break;
            case Instruction.BMI:
                BranchIfMinus(operand);
                break;
            case Instruction.BNE:
                BranchIfNotEqual(operand);
                break;
            case Instruction.BPL:
                BranchIfPositive(operand);
                break;
            // TODO: BRK
            case Instruction.BVC:
                BranchIfOverflowClear(operand);
                break;
            case Instruction.BVS:
                BranchIfOverflowSet(operand);
                break;
            case Instruction.CLC:
                _reg.C = false;
                break;
            case Instruction.CLD:
                _reg.D = false;
                break;
            case Instruction.CLI:
                _reg.I = false;
                break;
            case Instruction.CLV:
                _reg.V = false;
                break;
            case Instruction.CMP:
                _alu.Compare(operand);
                break;
            case Instruction.CPX:
                _alu.CompareX(operand);
                break;
            case Instruction.CPY:
                _alu.CompareY(operand);
                break;
            case Instruction.DEC:
                // TODO: Write result?
                _alu.Decrement(operand);
                break;
            case Instruction.LDA:
                _reg.A = operand;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void BranchIfCarryClear(byte value)
    {
        if (!_reg.C)
            BranchWithOffset(value);
    }

    private void BranchIfCarrySet(byte value)
    {
        if (_reg.C)
            BranchWithOffset(value);
    }

    private void BranchIfEqual(byte value)
    {
        if (_reg.Z)
            BranchWithOffset(value);
    }

    private void BranchIfMinus(byte value)
    {
        if (_reg.N)
            BranchWithOffset(value);
    }

    private void BranchIfNotEqual(byte value)
    {
        if (!_reg.Z)
            BranchWithOffset(value);
    }

    private void BranchIfPositive(byte value)
    {
        if (!_reg.N)
            BranchWithOffset(value);
    }

    private void BranchIfOverflowClear(byte value)
    {
        if (!_reg.V)
            BranchWithOffset(value);
    }

    private void BranchIfOverflowSet(byte value)
    {
        if (_reg.V)
            BranchWithOffset(value);
    }

    private void BranchWithOffset(byte value)
    {
        var offset = Cast(value);
        _cycles++;
        var mod = offset + (_reg.PC & 0xFF);
        if (mod is < 0 or > 256) _cycles++;
        _reg.PC = (ushort) (_reg.PC + offset);
    }

    private static byte Cast(sbyte value) =>
        MemoryMarshal.Cast<sbyte, byte>(stackalloc sbyte[] {value})[0];

    private static sbyte Cast(byte value) =>
        MemoryMarshal.Cast<byte, sbyte>(stackalloc byte[] {value})[0];
}
