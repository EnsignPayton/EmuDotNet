using EmuDotNet.Core.Abstractions;

namespace EmuDotNet.Core.MOS6502;

public class Processor : IProcessor
{
    private readonly Registers _reg = new();
    private readonly IBus _bus;
    private int _cycles;

    public Registers Registers => _reg;
    public IBus Bus => _bus;
    public bool IsExecuting => _cycles > 0;

    public Processor(IBus bus)
    {
        _bus = bus;
        Reset();
    }

    public void ExecuteClock()
    {
        if (!IsExecuting)
        {
            var opcodeByte = ReadImmediate();
            var opcode = OpcodeParser.Parse(opcodeByte);

            byte operand;
            if (opcode.AddressMode == AddressMode.IMM)
            {
                operand = ReadImmediate();
            }
            else if (opcode.AddressMode == AddressMode.ZPG)
            {
                operand = ReadZeroPage();
            }
            else
            {
                operand = ReadImplied();
            }

            if (opcode.Instruction == Instruction.ADC)
            {
                AddWithCarry(operand);
            }
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
        // TODO: This
    }

    public void InterruptNonMaskable()
    {
        // TODO: This
    }
    
    #region Bus Reads

    // 2 cycles
    private ushort ReadAbsolute()
    {
        var low = ReadImmediate();
        var high = ReadImmediate();
        return (ushort)((high << 8) | low);
    }

    // 4-5 cycles
    private ushort ReadAbsoluteXOffset(out int extraCycles) =>
        ReadAbsoluteOffset(_reg.X, out extraCycles);

    // 4-5 cycles
    private ushort ReadAbsoluteYOffset(out int extraCycles) =>
        ReadAbsoluteOffset(_reg.Y, out extraCycles);
    
    private ushort ReadAbsoluteOffset(byte reg, out int extraCycles)
    {
        var temp = ReadAbsolute();
        var result = (ushort)(temp + reg);
        extraCycles = (temp & 0xFF) + reg > 0xFF ? 1 : 0;
        return result;
    }

    private byte ReadImmediate()
    {
        var value = _bus[_reg.PC];
        _reg.PC += 1;
        _cycles++;
        return value;
    }

    private byte ReadImplied()
    {
        // Some opcodes use accumulator as implied value. It's the only one, so just return it
        return _reg.A;
    }

    private ushort ReadIndirect()
    {
        var temp = ReadAbsolute();

        // Do not cross page boundary - mod 256
        var highAddress = (temp & 0xFF) != 0xFF
            ? (ushort) (temp + 1)
            : (ushort) (temp & 0xFF00);

        var low = _bus[temp];
        var high = _bus[highAddress];

        return (ushort)((high << 8) | low);
    }
    
    // INX, INY

    private ushort ReadRelative()
    {
        // Return signed offset from PC, but still store it in a ushort for consistency
        ushort temp = ReadImmediate();
        return (temp & 0x80) != 0
            ? (ushort) (temp | 0xFF00)
            : temp;
    }

    private byte ReadZeroPage()
    {
        var address = ReadImmediate();
        var value = _bus[address];
        _cycles++;
        return value;
    }

    #endregion
    
    #region Opcode Execution

    private void AddWithCarry(byte value)
    {
        // A + M + C -> A, C
        var result = _reg.A + value + (_reg.C ? 1 : 0);
        var operandsSignMatch = (_reg.A & 0x80) == (value & 0x80);
        var resultSignMatch = (_reg.A & 0x80) == (result & 0x80);
        
        _reg.A = (byte) (result & 0xFF);
        _reg.C = (result & 0xFF00) != 0;
        _reg.Z = result == 0;
        _reg.V = operandsSignMatch && !resultSignMatch;
        _reg.N = (result & 0x80) > 0;
    }

    #endregion
}