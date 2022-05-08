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
    }

    public void ExecuteClock()
    {
        if (!IsExecuting)
        {
            var val = ReadImmediateByte();
            var (_, instruction, mode, cycles) = Opcode.Map[val];
            var extraCycles = Execute(instruction, mode);
            _cycles = cycles + extraCycles;
        }

        _cycles--;
    }

    #region Read Bus

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

    private byte ReadByte(AddressMode mode)
    {
        return ReadByte(mode, out _);
    }

    private byte ReadByte(AddressMode mode, out bool pageCross)
    {
         pageCross = false;
         return mode switch
         {
             AddressMode.IMM => ReadImmediateByte(),
             AddressMode.ZPG => _bus[ReadImmediateByte()],
             AddressMode.ZPX => _bus[(byte) ((ReadImmediateByte() + _reg.X) & 0xFF)],
             AddressMode.ABS => _bus[ReadImmediateUShort()],
             AddressMode.ABX => ReadAbsoluteX(out pageCross),
             AddressMode.ABY => ReadAbsoluteY(out pageCross),
             AddressMode.INX => ReadIndexedX(),
             AddressMode.INY => ReadIndexedY(out pageCross),
             _ => throw new NotImplementedException($"No value read defined for address mode {mode}")
         };
    }

    private byte ReadAbsoluteX(out bool pageCross)
    {
        var addressBase = ReadImmediateUShort();
        var address = (ushort) (addressBase + _reg.X);

        pageCross = (addressBase & 0xFF) + _reg.X > 256;
        return _bus[address];
    }

    private byte ReadAbsoluteY(out bool pageCross)
    {
        var addressBase = ReadImmediateUShort();
        var address = (ushort) (addressBase + _reg.Y);

        pageCross = (addressBase & 0xFF) + _reg.Y > 256;
        return _bus[address];
    }

    private byte ReadIndexedX()
    {
        var zAddressBase = ReadImmediateByte();
        var zAddress = (ushort) ((zAddressBase + _reg.X) & 0xFF);
        var address = ReadUShort(zAddress);

        return _bus[address];
    }

    private byte ReadIndexedY(out bool pageCross)
    {
        var zAddress = ReadImmediateByte();
        var addressBase = ReadUShort(zAddress);
        var address = (ushort) (addressBase + _reg.Y);

        pageCross = (addressBase & 0xFF) + _reg.Y > 256;
        return _bus[address];
    }

    private ushort ReadAddress(AddressMode mode) => mode switch
    {
        AddressMode.ZPG => ReadImmediateByte(),
        AddressMode.ZPX => (byte) ((ReadImmediateByte() + _reg.X) & 0xFF),
        AddressMode.ABS => ReadImmediateUShort(),
        AddressMode.ABX => (ushort) (ReadImmediateUShort() + _reg.X),
        _ => throw new NotImplementedException($"No address read defined for address mode {mode}")
    };

    #endregion

    private int Execute(Instruction instruction, AddressMode mode)
    {
        bool pageCross;
        switch (instruction)
        {
            case Instruction.ADC:
                _alu.AddWithCarry(ReadByte(mode, out pageCross));
                if (pageCross) return 1;
                break;
            case Instruction.AND:
                _alu.LogicalAnd(ReadByte(mode, out pageCross));
                if (pageCross) return 1;
                break;
            case Instruction.ASL:
                if (mode == AddressMode.IMP)
                {
                    _reg.A = _alu.ArithmeticShiftLeft(_reg.A);
                }
                else
                {
                    var address = ReadAddress(mode);
                    _bus[address] = _alu.ArithmeticShiftLeft(_bus[address]);
                }
                break;
            case Instruction.BCC:
            {
                var offset = ReadImmediateByte().ToSByte();
                if (!_reg.C) return Branch(offset);
                break;
            }
            case Instruction.BCS:
            {
                var offset = ReadImmediateByte().ToSByte();
                if (_reg.C) return Branch(offset);
                break;
            }
            case Instruction.BEQ:
            {
                var offset = ReadImmediateByte().ToSByte();
                if (_reg.Z) return Branch(offset);
                break;
            }
            case Instruction.BIT:
                _alu.BitTest(ReadByte(mode));
                break;
            case Instruction.BMI:
            {
                var offset = ReadImmediateByte().ToSByte();
                if (_reg.N) return Branch(offset);
                break;
            }
            case Instruction.BNE:
            {
                var offset = ReadImmediateByte().ToSByte();
                if (!_reg.Z) return Branch(offset);
                break;
            }
            case Instruction.BPL:
            {
                var offset = ReadImmediateByte().ToSByte();
                if (!_reg.N) return Branch(offset);
                break;
            }
            case Instruction.BRK:
                Break();
                break;
            case Instruction.BVC:
            {
                var offset = ReadImmediateByte().ToSByte();
                if (!_reg.V) return Branch(offset);
                break;
            }
            case Instruction.BVS:
            {
                var offset = ReadImmediateByte().ToSByte();
                if (_reg.V) return Branch(offset);
                break;
            }
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
                _alu.Compare(ReadByte(mode, out pageCross));
                if (pageCross) return 1;
                break;
            case Instruction.CPX:
                _alu.CompareX(ReadByte(mode, out pageCross));
                if (pageCross) return 1;
                break;
            case Instruction.CPY:
                _alu.CompareY(ReadByte(mode, out pageCross));
                if (pageCross) return 1;
                break;
            case Instruction.DEC:
            {
                var address = ReadAddress(mode);
                _bus[address] = _alu.Decrement(_bus[address]);
                break;
            }
            case Instruction.DEX:
                _alu.DecrementX();
                break;
            case Instruction.DEY:
                _alu.DecrementY();
                break;
            case Instruction.EOR:
                _alu.ExclusiveOr(ReadByte(mode, out pageCross));
                if (pageCross) return 1;
                break;
            case Instruction.INC:
            {
                var address = ReadAddress(mode);
                _bus[address] = _alu.Increment(_bus[address]);
                break;
            }
            case Instruction.INX:
                _alu.IncrementX();
                break;
            case Instruction.INY:
                _alu.IncrementY();
                break;
            case Instruction.JMP:
                _reg.PC = ReadAddress(mode);
                break;
            case Instruction.JSR:
                Push(_reg.PC);
                _reg.PC = ReadAddress(mode);
                break;
            case Instruction.LDA:
            {
                _reg.A = ReadByte(mode, out pageCross);
                if (pageCross) return 1;
                break;
            }
            case Instruction.LDX:
            {
                _reg.X = ReadByte(mode, out pageCross);
                if (pageCross) return 1;
                break;
            }
            case Instruction.LDY:
            {
                _reg.Y = ReadByte(mode, out pageCross);
                if (pageCross) return 1;
                break;
            }
            case Instruction.LSR:
                if (mode == AddressMode.IMP)
                {
                    _reg.A = _alu.LogicalShiftRight(_reg.A);
                }
                else
                {
                    var address = ReadAddress(mode);
                    _bus[address] = _alu.LogicalShiftRight(_bus[address]);
                }
                break;
            case Instruction.NOP:
                break;
            case Instruction.ORA:
                _alu.LogicalInclusiveOr(ReadByte(mode, out pageCross));
                if (pageCross) return 1;
                break;
            case Instruction.PHA:
                Push(_reg.A);
                break;
            case Instruction.PHP:
                _reg.B = true;
                Push(_reg.P);
                break;
            case Instruction.PLA:
                _reg.A = PullByte();
                break;
            case Instruction.PLP:
                _reg.P = PullByte();
                break;
            case Instruction.ROL:
                if (mode == AddressMode.IMP)
                {
                    _reg.A = _alu.RotateLeft(_reg.A);
                }
                else
                {
                    var address = ReadAddress(mode);
                    _bus[address] = _alu.RotateLeft(_bus[address]);
                }
                break;
            case Instruction.ROR:
                if (mode == AddressMode.IMP)
                {
                    _reg.A = _alu.RotateRight(_reg.A);
                }
                else
                {
                    var address = ReadAddress(mode);
                    _bus[address] = _alu.RotateRight(_bus[address]);
                }
                break;
            case Instruction.RTI:
            {
                _reg.P = PullByte();
                _reg.PC = PullUShort();
                break;
            }
            case Instruction.RTS:
            {
                _reg.PC = PullUShort();
                _reg.PC--;
                break;
            }
            case Instruction.SBC:
                _alu.SubtractWithCarry(ReadByte(mode, out pageCross));
                if (pageCross) return 1;
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
                _bus[ReadAddress(mode)] = _reg.A;
                break;
            case Instruction.STX:
                _bus[ReadAddress(mode)] = _reg.X;
                break;
            case Instruction.STY:
                _bus[ReadAddress(mode)] = _reg.Y;
                break;
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
                throw new NotImplementedException($"No implementation defined for instruction {instruction}");
        }

        return 0;
    }

    private int Branch(sbyte offset)
    {
        var mod = offset + (_reg.PC & 0xFF);
        _reg.PC = (ushort) (_reg.PC + offset);
        return mod is < 0 or > 256 ? 2 : 1;
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
