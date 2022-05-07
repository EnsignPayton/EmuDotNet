﻿using EmuDotNet.Core.Abstractions;

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
            var val = ReadImmediateByte();
            _cycles++;

            var (instruction, mode) = OpcodeParser.Parse(val);
            var operand = ReadOperand(mode, out var address);
            Execute(instruction, operand, address);
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

    private byte ReadImmediateByte()
    {
        return _bus[_reg.PC++];
    }

    private ushort ReadImmediateUShort()
    {
        var low = ReadImmediateByte();
        var high = ReadImmediateByte();
        return (ushort) ((high << 8) + low);
    }

    private ushort ReadUShort(ushort address)
    {
        var low = _bus[address];
        var high = _bus[(ushort) (address + 1)];
        return (ushort) ((high << 8) + low);
    }

    private byte ReadOperand(AddressMode mode, out ushort? outAddress)
    {
        outAddress = null;
        switch (mode)
        {
            case AddressMode.IMP:
            {
                _cycles++;
                return _reg.A;
            }
            case AddressMode.IMM:
            {
                _cycles++;
                return ReadImmediateByte();
            }
            case AddressMode.ZPG:
            {
                _cycles += 2;
                var address = ReadImmediateByte();
                outAddress = address;
                return _bus[address];
            }
            case AddressMode.ZPX:
            {
                _cycles += 3;
                var addressBase = ReadImmediateByte();
                var address = (byte)((addressBase + _reg.X) & 0xFF);
                return _bus[address];
            }
            case AddressMode.ABS:
            {
                _cycles += 3;
                var address = ReadImmediateUShort();
                outAddress = address;
                return _bus[address];
            }
            case AddressMode.ABX:
            {
                _cycles += 3;
                var addressBase = ReadImmediateUShort();
                var address = (ushort) (addressBase + _reg.X);
                if ((addressBase & 0xFF) + _reg.X > 256) _cycles++;
                return _bus[address];
            }
            case AddressMode.ABY:
            {
                _cycles += 3;
                var addressBase = ReadImmediateUShort();
                var address = (ushort) (addressBase + _reg.Y);
                if ((addressBase & 0xFF) + _reg.Y > 256) _cycles++;
                return _bus[address];
            }
            case AddressMode.INX:
            {
                _cycles += 5;
                var zAddressBase = ReadImmediateByte();
                var zAddress = (ushort) ((zAddressBase + _reg.X) & 0xFF);
                var address = ReadUShort(zAddress);
                return _bus[address];
            }
            case AddressMode.INY:
            {
                _cycles += 4;
                var zAddress = ReadImmediateByte();
                var addressBase = ReadUShort(zAddress);
                var address = (ushort) (addressBase + _reg.Y);
                if ((addressBase & 0xFF) + _reg.Y > 256) _cycles++;
                return _bus[address];
            }
            case AddressMode.REL:
            {
                _cycles++;
                return ReadImmediateByte();
            }
            default:
                throw new NotImplementedException();
        }
    }

    private void Execute(Instruction instruction, byte operand, ushort? address)
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
                if (address.HasValue)
                {
                    _bus[address.Value] = _alu.ArithmeticShiftLeft(operand);
                    _cycles += 2;
                }
                else
                {
                    _reg.A = _alu.ArithmeticShiftLeft(_reg.A);
                }
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
            case Instruction.BRK:
                Break();
                break;
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
            {
                var result = _alu.Decrement(operand);
                _cycles++;
                _bus[address!.Value] = result;
                _cycles++;
                break;
            }
            case Instruction.DEX:
                _alu.DecrementX();
                break;
            case Instruction.DEY:
                _alu.DecrementY();
                break;
            case Instruction.EOR:
                _alu.ExclusiveOr(operand);
                break;
            case Instruction.INC:
            {
                var result = _alu.Increment(operand);
                _cycles++;
                _bus[address!.Value] = result;
                _cycles++;
                break;
            }
            case Instruction.INX:
                _alu.IncrementX();
                break;
            case Instruction.INY:
                _alu.IncrementY();
                break;
            case Instruction.JMP:
                _reg.PC = address!.Value;
                // Didn't actually need to read the address - so don't maybe?
                _cycles--;
                break;
            case Instruction.JSR:
                Push(_reg.PC);
                _reg.PC = address!.Value;
                _cycles += 2;
                break;
            case Instruction.LDA:
                _reg.A = operand;
                break;
            case Instruction.LDX:
                _reg.X = operand;
                break;
            case Instruction.LDY:
                _reg.Y = operand;
                break;
            case Instruction.LSR:
                if (address.HasValue)
                {
                    var result = _alu.LogicalShiftRight(operand);
                    _bus[address.Value] = result;
                    _cycles += 2;
                }
                else
                {
                    _reg.A = _alu.LogicalShiftRight(operand);
                }
                break;
            case Instruction.NOP:
                break;
            case Instruction.ORA:
                _alu.LogicalInclusiveOr(operand);
                break;
            case Instruction.PHA:
                Push(_reg.A);
                _cycles++;
                break;
            case Instruction.PHP:
                _reg.B = true;
                Push(_reg.P);
                _cycles++;
                break;
            case Instruction.PLA:
                _reg.A = PullByte();
                _cycles += 2;
                break;
            case Instruction.PLP:
                _reg.P = PullByte();
                _cycles += 2;
                break;
            case Instruction.ROL:
                if (address.HasValue)
                {
                    var result = _alu.RotateLeft(operand);
                    _bus[address.Value] = result;
                    _cycles += 2;
                }
                else
                {
                    _reg.A = _alu.RotateLeft(_reg.A);
                }
                break;
            case Instruction.ROR:
                if (address.HasValue)
                {
                    var result = _alu.RotateRight(operand);
                    _bus[address.Value] = result;
                    _cycles += 2;
                }
                else
                {
                    _reg.A = _alu.RotateRight(_reg.A);
                }
                break;
            case Instruction.RTI:
            {
                _reg.P = PullByte();
                _reg.PC = PullUShort();
                _cycles += 4;
                break;
            }
            case Instruction.RTS:
            {
                _reg.PC = PullUShort();
                _reg.PC--;
                _cycles += 4;
                break;
            }
            case Instruction.SBC:
                _alu.SubtractWithCarry(operand);
                break;
            case Instruction.SEC:
                _alu.SetCarry();
                break;
            case Instruction.SED:
                _alu.SetDecimal();
                break;
            case Instruction.SEI:
                _alu.SetInterruptDisable();
                break;
            case Instruction.STA:
            {
                _bus[address!.Value] = _reg.A;
                break;
            }
            case Instruction.STX:
            {
                _bus[address!.Value] = _reg.X;
                break;
            }
            case Instruction.STY:
            {
                _bus[address!.Value] = _reg.Y;
                break;
            }
            case Instruction.TAX:
                _alu.TransferAtoX();
                break;
            case Instruction.TAY:
                _alu.TransferAtoY();
                break;
            case Instruction.TSX:
                _alu.TransferStoX();
                break;
            case Instruction.TXA:
                _alu.TransferXtoA();
                break;
            case Instruction.TXS:
                _alu.TransferXtoS();
                break;
            case Instruction.TYA:
                _alu.TransferYtoA();
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
        var offset = value.ToSByte();
        _cycles++;
        var mod = offset + (_reg.PC & 0xFF);
        if (mod is < 0 or > 256) _cycles++;
        _reg.PC = (ushort) (_reg.PC + offset);
    }

    private void Break()
    {
        Push(_reg.PC);
        _reg.B = true;
        Push(_reg.P);

        var low = _bus[0xFFFE];
        var high = _bus[0xFFFF];
        var address = (ushort) ((high << 8) + low);
        _reg.PC = address;
        _cycles += 5;
    }

    private void Push(byte value)
    {
        _bus[(ushort) (0x0100 + _reg.SP)] = value;
        _reg.SP--;
    }

    private void Push(ushort value)
    {
        Push((byte) (value & 0xFF));
        Push((byte) ((value >> 8) & 0xFF));
    }

    private byte PullByte()
    {
        var value = _bus[(ushort) (0x0100 + _reg.SP)];
        _reg.SP++;
        return value;
    }

    private ushort PullUShort()
    {
        var high = PullByte();
        var low = PullByte();
        return (ushort) ((high << 8) + low);
    }
}
