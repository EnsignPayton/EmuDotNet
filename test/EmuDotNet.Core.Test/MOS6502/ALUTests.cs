namespace EmuDotNet.Core.MOS6502;

public class ALUTests
{
    #region ADC

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 1, 2)]
    [InlineData(1, 2, 3)]
    [InlineData(10, 20, 30)]
    [InlineData(123, 123, 246)]
    public void ADC_Adds_Bytes(byte a, byte m, byte expected)
    {
        var target = GetTarget();
        target.Registers.A = a;
        target.AddWithCarry(m);
        Assert.Equal(expected, target.Registers.A);
    }

    [Theory]
    [InlineData(1, -1, 0)]
    [InlineData(45, -20, 25)]
    [InlineData(-50, -50, -100)]
    [InlineData(-100, 125, 25)]
    public void ADC_Adds_SBytes(sbyte a, sbyte m, sbyte expected)
    {
        var target = GetTarget();
        target.Registers.A = a.ToByte();
        target.AddWithCarry(m.ToByte());
        Assert.Equal(expected, target.Registers.A.ToSByte());
    }

    [Fact]
    public void ADC_Adds_Carry()
    {
        var target = GetTarget();
        target.Registers.A = 100;
        target.Registers.C = true;
        target.AddWithCarry(100);
        Assert.Equal(201, target.Registers.A);
    }

    [Fact]
    public void ADC_Sets_C()
    {
        var target = GetTarget();
        target.Registers.A = 200;
        target.AddWithCarry(200);
        Assert.True(target.Registers.C);
    }

    [Fact]
    public void ADC_Sets_Z()
    {
        var target = GetTarget();
        target.AddWithCarry(0);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void ADC_Sets_V()
    {
        const sbyte a = 127;
        const sbyte m = 127;
        var target = GetTarget();
        target.Registers.A = a.ToByte();
        target.AddWithCarry(m.ToByte());
        Assert.True(target.Registers.V);
    }

    [Fact]
    public void ADC_Sets_N()
    {
        const sbyte a = -50;
        const sbyte m = -50;
        var target = GetTarget();
        target.Registers.A = a.ToByte();
        target.AddWithCarry(m.ToByte());
        Assert.True(target.Registers.N);
    }

    #endregion

    #region AND

    [Theory]
    [InlineData(0b00000000, 0b00000000, 0b00000000)]
    [InlineData(0b00000000, 0b11111111, 0b00000000)]
    [InlineData(0b01010101, 0b10101010, 0b00000000)]
    [InlineData(0b11110000, 0b10101010, 0b10100000)]
    [InlineData(0b11111111, 0b11001100, 0b11001100)]
    [InlineData(0b11111111, 0b11111111, 0b11111111)]
    public void AND_Ands_Bytes(byte a, byte m, byte expected)
    {
        var target = GetTarget();
        target.Registers.A = a;
        target.LogicalAnd(m);
        Assert.Equal(expected, target.Registers.A);
    }

    [Fact]
    public void AND_Sets_Z()
    {
        var target = GetTarget();
        target.LogicalAnd(0);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void AND_Sets_N()
    {
        var target = GetTarget();
        target.Registers.A = 0xFF;
        target.LogicalAnd(0x80);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region ASL

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 2)]
    [InlineData(32, 64)]
    [InlineData(31, 62)]
    [InlineData(127, 254)]
    [InlineData(200, 144)]
    public void ASL_Shifts(byte m, byte expected)
    {
        var target = GetTarget();
        var result = target.ArithmeticShiftLeft(m);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ASL_Sets_C()
    {
        var target = GetTarget();
        target.ArithmeticShiftLeft(0x80);
        Assert.True(target.Registers.C);
    }

    [Fact]
    public void ASL_Sets_Z()
    {
        var target = GetTarget();
        target.ArithmeticShiftLeft(0);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void ASL_Sets_N()
    {
        var target = GetTarget();
        target.ArithmeticShiftLeft(0x40);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region BIT

    [Fact]
    public void BIT_Tests_Mask()
    {
        var target = GetTarget();

        for (int i = 0; i < 7; i++)
        {
            target.Registers.A = (byte)(1 << i);
            target.BitTest(0b10101010);
            Assert.Equal(i % 2 == 0, target.Registers.Z);
        }
    }

    [Fact]
    public void BIT_Sets_V()
    {
        var target = GetTarget();
        target.BitTest(0b01000000);
        Assert.True(target.Registers.V);
    }

    [Fact]
    public void BIT_Sets_N()
    {
        var target = GetTarget();
        target.BitTest(0b10000000);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region Clear Flags

    [Fact]
    public void CLC_Clears_C()
    {
        var target = GetTarget();
        target.Registers.C = true;
        target.ClearCarry();
        Assert.False(target.Registers.C);
    }

    [Fact]
    public void CLD_Clears_D()
    {
        var target = GetTarget();
        target.Registers.D = true;
        target.ClearDecimal();
        Assert.False(target.Registers.D);
    }

    [Fact]
    public void CLI_Clears_I()
    {
        var target = GetTarget();
        target.Registers.I = true;
        target.ClearInterruptDisable();
        Assert.False(target.Registers.I);
    }

    [Fact]
    public void CLV_Clears_V()
    {
        var target = GetTarget();
        target.Registers.V = true;
        target.ClearOverflow();
        Assert.False(target.Registers.V);
    }

    #endregion

    #region CMP

    [Fact]
    public void CMP_Sets_C()
    {
        var target = GetTarget();
        target.Registers.A = 100;
        target.Compare(50);
        Assert.True(target.Registers.C);
    }

    [Fact]
    public void CMP_Sets_Z()
    {
        var target = GetTarget();
        target.Registers.A = 25;
        target.Compare(25);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void CMP_Sets_N()
    {
        var target = GetTarget();
        target.Registers.A = 0xFF;
        target.Compare(1);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region CPX

    [Fact]
    public void CPX_Sets_C()
    {
        var target = GetTarget();
        target.Registers.X = 100;
        target.CompareX(50);
        Assert.True(target.Registers.C);
    }

    [Fact]
    public void CPX_Sets_Z()
    {
        var target = GetTarget();
        target.Registers.X = 25;
        target.CompareX(25);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void CPX_Sets_N()
    {
        var target = GetTarget();
        target.Registers.X = 0xFF;
        target.CompareX(1);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region CPY

    [Fact]
    public void CPY_Sets_C()
    {
        var target = GetTarget();
        target.Registers.Y = 100;
        target.CompareY(50);
        Assert.True(target.Registers.C);
    }

    [Fact]
    public void CPY_Sets_Z()
    {
        var target = GetTarget();
        target.Registers.Y = 25;
        target.CompareY(25);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void CPY_Sets_N()
    {
        var target = GetTarget();
        target.Registers.Y = 0xFF;
        target.CompareY(1);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region DEC

    [Theory]
    [InlineData(25, 24)]
    [InlineData(250, 249)]
    [InlineData(0, 255)]
    public void DEC_Decrements(byte value, byte expected)
    {
        var target = GetTarget();
        var result = target.Decrement(value);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(25, 24)]
    [InlineData(250, 249)]
    [InlineData(0, -1)]
    [InlineData(-20, -21)]
    public void DEC_Decrements_Signed(int iValue, int iExpected)
    {
        var value = (sbyte)iValue;
        var expected = (sbyte)iExpected;
        var target = GetTarget();
        var result = target.Decrement(value.ToByte()).ToSByte();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DEC_Sets_Z()
    {
        var target = GetTarget();
        target.Decrement(1);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void DEC_Sets_N()
    {
        const sbyte m = -25;
        var target = GetTarget();
        target.Decrement(m.ToByte());
        Assert.True(target.Registers.N);
    }

    #endregion

    #region DEX

    [Theory]
    [InlineData(25, 24)]
    [InlineData(250, 249)]
    [InlineData(0, 255)]
    public void DEX_Decrements(byte value, byte expected)
    {
        var target = GetTarget();
        target.Registers.X = value;
        target.DecrementX();
        var result = target.Registers.X;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(25, 24)]
    [InlineData(250, 249)]
    [InlineData(0, -1)]
    [InlineData(-20, -21)]
    public void DEX_Decrements_Signed(int iValue, int iExpected)
    {
        var value = (sbyte)iValue;
        var expected = (sbyte)iExpected;
        var target = GetTarget();
        target.Registers.X = value.ToByte();
        target.DecrementX();
        var result = target.Registers.X.ToSByte();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DEX_Sets_Z()
    {
        var target = GetTarget();
        target.Registers.X = 1;
        target.DecrementX();
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void DEX_Sets_N()
    {
        const sbyte m = -25;
        var target = GetTarget();
        target.Registers.X = m.ToByte();
        target.DecrementX();
        Assert.True(target.Registers.N);
    }

    #endregion

    #region DEY

    [Theory]
    [InlineData(25, 24)]
    [InlineData(250, 249)]
    [InlineData(0, 255)]
    public void DEY_Decrements(byte value, byte expected)
    {
        var target = GetTarget();
        target.Registers.Y = value;
        target.DecrementY();
        var result = target.Registers.Y;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(25, 24)]
    [InlineData(250, 249)]
    [InlineData(0, -1)]
    [InlineData(-20, -21)]
    public void DEY_Decrements_Signed(int iValue, int iExpected)
    {
        var value = (sbyte)iValue;
        var expected = (sbyte)iExpected;
        var target = GetTarget();
        target.Registers.Y = value.ToByte();
        target.DecrementY();
        var result = target.Registers.Y.ToSByte();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DEY_Sets_Z()
    {
        var target = GetTarget();
        target.Registers.Y = 1;
        target.DecrementY();
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void DEY_Sets_N()
    {
        const sbyte m = -25;
        var target = GetTarget();
        target.Registers.Y = m.ToByte();
        target.DecrementY();
        Assert.True(target.Registers.N);
    }

    #endregion

    #region EOR

    [Theory]
    [InlineData(0b00000000, 0b10101010, 0b10101010)]
    [InlineData(0b11111111, 0b10101010, 0b01010101)]
    [InlineData(0b11110000, 0b11001100, 0b00111100)]
    public void EOR_Ors(byte a, byte m, byte expected)
    {
        var target = GetTarget();
        target.Registers.A = a;
        target.ExclusiveOr(m);
        Assert.Equal(expected, target.Registers.A);
    }

    [Fact]
    public void EOR_Sets_Z()
    {
        var target = GetTarget();
        target.ExclusiveOr(0);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void EOR_Sets_N()
    {
        var target = GetTarget();
        target.ExclusiveOr(0x80);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region INC

    [Theory]
    [InlineData(25, 26)]
    [InlineData(250, 251)]
    [InlineData(255, 0)]
    public void INC_Increments(byte value, byte expected)
    {
        var target = GetTarget();
        var result = target.Increment(value);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(25, 26)]
    [InlineData(250, 251)]
    [InlineData(-1, 0)]
    [InlineData(-21, -20)]
    public void INC_Increments_Signed(int iValue, int iExpected)
    {
        var value = (sbyte)iValue;
        var expected = (sbyte)iExpected;
        var target = GetTarget();
        var result = target.Increment(value.ToByte()).ToSByte();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void INC_Sets_Z()
    {
        var target = GetTarget();
        target.Increment(255);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void INC_Sets_N()
    {
        const sbyte m = -25;
        var target = GetTarget();
        target.Increment(m.ToByte());
        Assert.True(target.Registers.N);
    }

    #endregion

    #region INX

    [Theory]
    [InlineData(25, 26)]
    [InlineData(250, 251)]
    [InlineData(255, 0)]
    public void INX_Increments(byte value, byte expected)
    {
        var target = GetTarget();
        target.Registers.X = value;
        target.IncrementX();
        var result = target.Registers.X;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(25, 26)]
    [InlineData(250, 251)]
    [InlineData(-1, 0)]
    [InlineData(-21, -20)]
    public void INX_Increments_Signed(int iValue, int iExpected)
    {
        var value = (sbyte)iValue;
        var expected = (sbyte)iExpected;
        var target = GetTarget();
        target.Registers.X = value.ToByte();
        target.IncrementX();
        var result = target.Registers.X.ToSByte();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void INX_Sets_Z()
    {
        var target = GetTarget();
        target.Registers.X = 255;
        target.IncrementX();
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void INX_Sets_N()
    {
        const sbyte m = -25;
        var target = GetTarget();
        target.Registers.X = m.ToByte();
        target.IncrementX();
        Assert.True(target.Registers.N);
    }

    #endregion

    #region INY

    [Theory]
    [InlineData(25, 26)]
    [InlineData(250, 251)]
    [InlineData(255, 0)]
    public void INY_Increments(byte value, byte expected)
    {
        var target = GetTarget();
        target.Registers.Y = value;
        target.IncrementY();
        var result = target.Registers.Y;
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(25, 26)]
    [InlineData(250, 251)]
    [InlineData(-1, 0)]
    [InlineData(-21, -20)]
    public void INY_Increments_Signed(int iValue, int iExpected)
    {
        var value = (sbyte)iValue;
        var expected = (sbyte)iExpected;
        var target = GetTarget();
        target.Registers.Y = value.ToByte();
        target.IncrementY();
        var result = target.Registers.Y.ToSByte();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void INY_Sets_Z()
    {
        var target = GetTarget();
        target.Registers.Y = 255;
        target.IncrementY();
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void INY_Sets_N()
    {
        const sbyte m = -25;
        var target = GetTarget();
        target.Registers.Y = m.ToByte();
        target.IncrementY();
        Assert.True(target.Registers.N);
    }

    #endregion

    #region LDA

    [Fact]
    public void LDA_Loads_Value()
    {
        var target = GetTarget();
        target.LoadA(0xA5);
        Assert.Equal(0xA5, target.Registers.A);
    }

    [Fact]
    public void LDA_Sets_Z()
    {
        var target = GetTarget();
        target.LoadA(0x00);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void LDA_Sets_N()
    {
        var target = GetTarget();
        target.LoadA(0x80);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region LDX

    [Fact]
    public void LDX_Loads_Value()
    {
        var target = GetTarget();
        target.LoadX(0xA5);
        Assert.Equal(0xA5, target.Registers.X);
    }

    [Fact]
    public void LDX_Sets_Z()
    {
        var target = GetTarget();
        target.LoadX(0x00);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void LDX_Sets_N()
    {
        var target = GetTarget();
        target.LoadX(0x80);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region LDY

    [Fact]
    public void LDY_Loads_Value()
    {
        var target = GetTarget();
        target.LoadY(0xA5);
        Assert.Equal(0xA5, target.Registers.Y);
    }

    [Fact]
    public void LDY_Sets_Z()
    {
        var target = GetTarget();
        target.LoadY(0x00);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void LDY_Sets_N()
    {
        var target = GetTarget();
        target.LoadY(0x80);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region LSR

    [Theory]
    [InlineData(0, 0)]
    [InlineData(2, 1)]
    [InlineData(64, 32)]
    [InlineData(65, 32)]
    [InlineData(254, 127)]
    public void LSR_Shifts(byte m, byte expected)
    {
        var target = GetTarget();
        var result = target.LogicalShiftRight(m);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void LSR_Sets_C()
    {
        var target = GetTarget();
        target.LogicalShiftRight(0x01);
        Assert.True(target.Registers.C);
    }

    [Fact]
    public void LSR_Sets_Z()
    {
        var target = GetTarget();
        target.LogicalShiftRight(0);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void LSR_Clears_N()
    {
        var target = GetTarget();
        target.LogicalShiftRight(0xFF);
        Assert.False(target.Registers.N);
    }

    #endregion

    #region ORA

    [Theory]
    [InlineData(0b00000000, 0b10101010, 0b10101010)]
    [InlineData(0b11110000, 0b11001100, 0b11111100)]
    [InlineData(0b00000001, 0b10000000, 0b10000001)]
    public void ORA_Ors(byte a, byte m, byte expected)
    {
        var target = GetTarget();
        target.Registers.A = a;
        target.LogicalInclusiveOr(m);
        Assert.Equal(expected, target.Registers.A);
    }

    [Fact]
    public void ORA_Sets_Z()
    {
        var target = GetTarget();
        target.LogicalInclusiveOr(0);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void ORA_Sets_N()
    {
        var target = GetTarget();
        target.LogicalInclusiveOr(0x80);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region ROL

    [Fact]
    public void ROL_Rotates_Inner()
    {
        var target = GetTarget();
        var result = target.RotateLeft(0b00011000);
        Assert.Equal(0b00110000, result);
    }

    [Fact]
    public void ROL_Rotates_Carry_In()
    {
        var target = GetTarget();
        target.Registers.C = true;
        var result = target.RotateLeft(0b00011000);
        Assert.Equal(0b00110001, result);
    }

    [Fact]
    public void ROL_Rotates_Carry_Out()
    {
        var target = GetTarget();
        target.RotateLeft(0x80);
        Assert.True(target.Registers.C);
    }

    [Fact]
    public void ROL_Sets_Z()
    {
        var target = GetTarget();
        target.RotateLeft(0x80);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void ROL_Sets_N()
    {
        var target = GetTarget();
        target.RotateLeft(0x40);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region ROR

    [Fact]
    public void ROR_Rotates_Inner()
    {
        var target = GetTarget();
        var result = target.RotateRight(0b00011000);
        Assert.Equal(0b00001100, result);
    }

    [Fact]
    public void ROR_Rotates_Carry_In()
    {
        var target = GetTarget();
        target.Registers.C = true;
        var result = target.RotateRight(0b00011000);
        Assert.Equal(0b10001100, result);
    }

    [Fact]
    public void ROR_Rotates_Carry_Out()
    {
        var target = GetTarget();
        target.RotateRight(0x01);
        Assert.True(target.Registers.C);
    }

    [Fact]
    public void ROR_Sets_Z()
    {
        var target = GetTarget();
        target.RotateRight(0x01);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void ROR_Sets_N()
    {
        var target = GetTarget();
        target.Registers.C = true;
        target.RotateRight(0x00);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region SBC

    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(1, 1, 0)]
    [InlineData(2, 1, 1)]
    [InlineData(30, 20, 10)]
    [InlineData(255, 100, 155)]
    [InlineData(100, 200, 156)]
    public void SBC_Subtracts_Bytes(byte a, byte m, byte expected)
    {
        var target = GetTarget();
        target.Registers.A = a;
        target.Registers.C = true;
        target.SubtractWithCarry(m);
        Assert.Equal(expected, target.Registers.A);
    }

    [Theory]
    [InlineData(1, -1, 2)]
    [InlineData(-30, -20, -10)]
    [InlineData(50, 60, -10)]
    [InlineData(100, 100, 0)]
    public void SBC_Subtracts_SBytes(sbyte a, sbyte m, sbyte expected)
    {
        var target = GetTarget();
        target.Registers.A = a.ToByte();
        target.Registers.C = true;
        target.SubtractWithCarry(m.ToByte());
        Assert.Equal(expected, target.Registers.A.ToSByte());
    }

    [Fact]
    public void SBC_Subtracts_Carry()
    {
        var target = GetTarget();
        target.Registers.A = 100;
        target.Registers.C = false;
        target.SubtractWithCarry(50);
        Assert.Equal(49, target.Registers.A);
    }

    [Fact]
    public void SBC_Sets_C()
    {
        var target = GetTarget();
        target.Registers.A = 0x50;
        target.Registers.C = true;
        target.SubtractWithCarry(0x30);
        Assert.True(target.Registers.C);
    }

    [Fact]
    public void SBC_Sets_Z()
    {
        var target = GetTarget();
        target.Registers.C = true;
        target.SubtractWithCarry(0);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void SBC_Sets_V()
    {
        const sbyte a = 127;
        const sbyte m = 127;
        var target = GetTarget();
        target.Registers.A = a.ToByte();
        target.AddWithCarry(m.ToByte());
        Assert.True(target.Registers.V);
    }

    [Fact]
    public void SBC_Sets_N()
    {
        var target = GetTarget();
        target.Registers.A = 0xFF;
        target.SubtractWithCarry(0);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region Set Flags

    [Fact]
    public void SEC_Sets_C()
    {
        var target = GetTarget();
        target.SetCarry();
        Assert.True(target.Registers.C);
    }

    [Fact]
    public void SED_Sets_D()
    {
        var target = GetTarget();
        target.SetDecimal();
        Assert.True(target.Registers.D);
    }

    [Fact]
    public void SEI_Sets_I()
    {
        var target = GetTarget();
        target.SetInterruptDisable();
        Assert.True(target.Registers.I);
    }

    #endregion

    #region Transfers

    [Fact]
    public void TAX_Sets_X()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.TransferAtoX();
        Assert.Equal(0xA5, target.Registers.X);
    }

    [Fact]
    public void TAX_Sets_Z()
    {
        var target = GetTarget();
        target.TransferAtoX();
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void TAX_Sets_N()
    {
        var target = GetTarget();
        target.Registers.A = 0x80;
        target.TransferAtoX();
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void TAY_Sets_Y()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.TransferAtoY();
        Assert.Equal(0xA5, target.Registers.Y);
    }

    [Fact]
    public void TAY_Sets_Z()
    {
        var target = GetTarget();
        target.TransferAtoY();
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void TAY_Sets_N()
    {
        var target = GetTarget();
        target.Registers.A = 0x80;
        target.TransferAtoY();
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void TSX_Sets_X()
    {
        var target = GetTarget();
        target.Registers.SP = 0xA5;
        target.TransferStoX();
        Assert.Equal(0xA5, target.Registers.X);
    }

    [Fact]
    public void TSX_Sets_Z()
    {
        var target = GetTarget();
        target.Registers.SP = 0;
        target.TransferStoX();
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void TSX_Sets_N()
    {
        var target = GetTarget();
        target.Registers.SP = 0x80;
        target.TransferStoX();
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void TXA_Sets_A()
    {
        var target = GetTarget();
        target.Registers.X = 0xA5;
        target.TransferXtoA();
        Assert.Equal(0xA5, target.Registers.A);
    }

    [Fact]
    public void TXA_Sets_Z()
    {
        var target = GetTarget();
        target.TransferXtoA();
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void TXA_Sets_N()
    {
        var target = GetTarget();
        target.Registers.X = 0x80;
        target.TransferXtoA();
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void TXS_Sets_SP()
    {
        var target = GetTarget();
        target.Registers.X = 0xA5;
        target.TransferXtoS();
        Assert.Equal(0xA5, target.Registers.SP);
    }

    [Fact]
    public void TYA_Sets_A()
    {
        var target = GetTarget();
        target.Registers.Y = 0xA5;
        target.TransferYtoA();
        Assert.Equal(0xA5, target.Registers.A);
    }

    [Fact]
    public void TYA_Sets_Z()
    {
        var target = GetTarget();
        target.TransferYtoA();
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void TYA_Sets_N()
    {
        var target = GetTarget();
        target.Registers.Y = 0x80;
        target.TransferYtoA();
        Assert.True(target.Registers.N);
    }

    #endregion

    private static ALU GetTarget() => new(new Registers());
    // private static byte Cast(sbyte value) => value.ToByte();
    // private static sbyte Cast(byte value) => value.ToSByte();
}
