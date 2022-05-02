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
                return _reg.A;
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
            default:
                throw new NotImplementedException();
        }
    }

    private void Execute(Instruction instruction, byte operand)
    {
        if (false)
        {
        }
        else if (instruction == Instruction.ADC)
        {
            _alu.AddWithCarry(operand);
        }
        else if (instruction == Instruction.LDA)
        {
            _reg.A = operand;
        }
        else
        {
            throw new NotImplementedException();
        }
    }
}
