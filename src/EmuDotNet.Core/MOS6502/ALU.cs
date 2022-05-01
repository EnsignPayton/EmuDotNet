namespace EmuDotNet.Core.MOS6502;

/// <summary>
/// Performs internal CPU operations. No instruction decode or bus operation capabilities.
/// </summary>
public class ALU
{
    private readonly Registers _reg;

    public Registers Registers => _reg;

    public ALU(Registers reg)
    {
        _reg = reg;
    }

    // ADC
    public void AddWithCarry(byte value)
    {
        var result = _reg.A + value + (_reg.C ? 1 : 0);
        _reg.C = (result & 0x0100) > 0;
        _reg.Z = result == 0;
        _reg.V = (_reg.A & 0x80) == 0 && (value & 0x80) == 0 && (result & 0x80) != 0 ||
                 (_reg.A & 0x80) != 0 && (value & 0x80) != 0 && (result & 0x80) == 0;
        _reg.N = (result & 0x80) > 0;
        _reg.A = (byte)(result & 0xFF);
    }

    // AND
    public void LogicalAnd(byte value)
    {
        var result = _reg.A & value;
        _reg.Z = result == 0;
        _reg.N = (result & 0x80) != 0;
        _reg.A = (byte)(result & 0xFF);
    }

    // ASL
    public byte ArithmeticShiftLeft(byte value)
    {
        var result = value << 1;
        _reg.C = (result & 0x0100) != 0;
        _reg.Z = (result & 0xFF) == 0;
        _reg.N = (result & 0x80) != 0;
        return (byte)(result & 0xFF);
    }

    // BIT
    public void BitTest(byte value)
    {
        var test = _reg.A & value;
        _reg.V = (value & 0x40) != 0;
        _reg.N = (value & 0x80) != 0;
        _reg.Z = (test & 0xFF) == 0;
    }

    // CLC
    public void ClearCarry()
    {
        _reg.C = false;
    }

    // CLD
    public void ClearDecimal()
    {
        _reg.D = false;
    }

    // CLI
    public void ClearInterruptDisable()
    {
        _reg.I = false;
    }

    // CLV
    public void ClearOverflow()
    {
        _reg.V = false;
    }

    // CMP
    public void Compare(byte value)
    {
        Compare(_reg.A, value);
    }

    // CPX
    public void CompareX(byte value)
    {
        Compare(_reg.X, value);
    }

    // CPY
    public void CompareY(byte value)
    {
        Compare(_reg.Y, value);
    }

    private void Compare(byte regValue, byte memValue)
    {
        var result = regValue - memValue;
        _reg.C = result > 0;
        _reg.Z = result == 0;
        _reg.N = (result & 0x80) != 0;
    }

    // DEC
    public byte Decrement(byte value)
    {
        var result = value - 1;
        _reg.Z = result == 0;
        _reg.N = (result & 0x80) != 0;
        return (byte)(result & 0xFF);
    }

    // DEX
    public void DecrementX()
    {
        _reg.X = Decrement(_reg.X);
    }

    // DEY
    public void DecrementY()
    {
        _reg.Y = Decrement(_reg.Y);
    }

    // EOR
    public void ExclusiveOr(byte value)
    {
        var result = _reg.A ^ value;
        _reg.Z = result == 0;
        _reg.N = (result & 0x80) != 0;
        _reg.A = (byte)(result & 0xFF);
    }

    // INC
    public byte Increment(byte value)
    {
        var result = value + 1;
        _reg.Z = (result & 0xFF) == 0;
        _reg.N = (result & 0x80) != 0;
        return (byte)(result & 0xFF);
    }

    // INX
    public void IncrementX()
    {
        _reg.X = Increment(_reg.X);
    }

    // INY
    public void IncrementY()
    {
        _reg.Y = Increment(_reg.Y);
    }

    // LDA
    public void LoadA(byte value)
    {
        _reg.Z = value == 0;
        _reg.N = (value & 0x80) != 0;
        _reg.A = value;
    }

    // LDX
    public void LoadX(byte value)
    {
        _reg.Z = value == 0;
        _reg.N = (value & 0x80) != 0;
        _reg.X = value;
    }

    // LDY
    public void LoadY(byte value)
    {
        _reg.Z = value == 0;
        _reg.N = (value & 0x80) != 0;
        _reg.Y = value;
    }

    // LSR
    public byte LogicalShiftRight(byte value)
    {
        var result = value >> 1;
        _reg.C = (value & 0x01) != 0;
        _reg.Z = (result & 0xFF) == 0;
        _reg.N = (result & 0x80) != 0;
        return (byte)(result & 0xFF);
    }

    // ORA
    public void LogicalInclusiveOr(byte value)
    {
        var result = _reg.A | value;
        _reg.Z = result == 0;
        _reg.N = (result & 0x80) != 0;
        _reg.A = (byte)(result & 0xFF);
    }

    // ROL
    public byte RotateLeft(byte value)
    {
        var result = (value << 1) + (_reg.C ? 1 : 0);
        _reg.C = (value & 0x80) != 0;
        _reg.Z = (result & 0xFF) == 0;
        _reg.N = (result & 0x80) != 0;
        return (byte)(result & 0xFF);
    }

    // ROR
    public byte RotateRight(byte value)
    {
        var result = (value >> 1) + (_reg.C ? 0x80 : 0x00);
        _reg.C = (value & 0x01) != 0;
        _reg.Z = (result & 0xFF) == 0;
        _reg.N = (result & 0x80) != 0;
        return (byte) (result & 0xFF);
    }

    // SBC
    public void SubtractWithCarry(byte value)
    {
        var comp = (byte) (value ^ 0xFF);
        AddWithCarry(comp);
        _reg.Z = _reg.A == 0;
    }

    // SEC
    public void SetCarry()
    {
        _reg.C = true;
    }

    // SED
    public void SetDecimal()
    {
        _reg.D = true;
    }

    // SEI
    public void SetInterruptDisable()
    {
        _reg.I = true;
    }

    // TAX
    public void TransferAtoX()
    {
        _reg.X = _reg.A;
        SetTransferFlags(_reg.X);
    }

    // TAY
    public void TransferAtoY()
    {
        _reg.Y = _reg.A;
        SetTransferFlags(_reg.Y);
    }

    // TSX
    public void TransferStoX()
    {
        _reg.X = _reg.SP;
        SetTransferFlags(_reg.X);
    }

    // TXA
    public void TransferXtoA()
    {
        _reg.A = _reg.X;
        SetTransferFlags(_reg.A);
    }

    // TXS
    public void TransferXtoS()
    {
        _reg.SP = _reg.X;
    }

    // TYA
    public void TransferYtoA()
    {
        _reg.A = _reg.Y;
        SetTransferFlags(_reg.A);
    }

    private void SetTransferFlags(byte value)
    {
        _reg.Z = value == 0;
        _reg.N = (value & 0x80) != 0;
    }
}
