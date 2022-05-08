namespace EmuDotNet.Core.MOS6502;

public class ProcessorTests
{
    #region ADC

    [Fact]
    public void ADC_IMM()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0x69;
        target.Bus[0x8001] = 0x11;

        ExecuteCycles(target, 2);

        Assert.Equal(0xB6, target.Registers.A);
    }

    [Fact]
    public void ADC_ZPG()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x0015] = 0x11;
        target.Bus[0x8000] = 0x65;
        target.Bus[0x8001] = 0x15;

        ExecuteCycles(target, 3);

        Assert.Equal(0xB6, target.Registers.A);
    }

    [Fact]
    public void ADC_ZPX()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.X = 0x04;
        target.Bus[0x0019] = 0x11;
        target.Bus[0x8000] = 0x75;
        target.Bus[0x8001] = 0x15;

        ExecuteCycles(target, 4);

        Assert.Equal(0xB6, target.Registers.A);
    }

    [Fact]
    public void ADC_ABS()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x1234] = 0x11;
        target.Bus[0x8000] = 0x6D;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 4);

        Assert.Equal(0xB6, target.Registers.A);
    }

    [Fact]
    public void ADC_ABX()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.X = 0x04;
        target.Bus[0x1238] = 0x11;
        target.Bus[0x8000] = 0x7D;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 4);

        Assert.Equal(0xB6, target.Registers.A);
    }

    [Fact]
    public void ADC_ABX_PageCross()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.X = 0x04;
        target.Bus[0x1203] = 0x11;
        target.Bus[0x8000] = 0x7D;
        target.Bus[0x8001] = 0xFF;
        target.Bus[0x8002] = 0x11;

        ExecuteCycles(target, 5);

        Assert.Equal(0xB6, target.Registers.A);
    }

    [Fact]
    public void ADC_ABY()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.Y = 0x04;
        target.Bus[0x1238] = 0x11;
        target.Bus[0x8000] = 0x79;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 4);

        Assert.Equal(0xB6, target.Registers.A);
    }

    [Fact]
    public void ADC_ABY_PageCross()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.Y = 0x04;
        target.Bus[0x1203] = 0x11;
        target.Bus[0x8000] = 0x79;
        target.Bus[0x8001] = 0xFF;
        target.Bus[0x8002] = 0x11;

        ExecuteCycles(target, 5);

        Assert.Equal(0xB6, target.Registers.A);
    }

    [Fact]
    public void ADC_INX()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.X = 0x04;
        target.Bus[0x0014] = 0x34;
        target.Bus[0x0015] = 0x12;
        target.Bus[0x1234] = 0x11;
        target.Bus[0x8000] = 0x61;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 6);

        Assert.Equal(0xB6, target.Registers.A);
    }

    [Fact]
    public void ADC_INY()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.Y = 0x04;
        target.Bus[0x0010] = 0x34;
        target.Bus[0x0011] = 0x12;
        target.Bus[0x1238] = 0x11;
        target.Bus[0x8000] = 0x71;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 5);

        Assert.Equal(0xB6, target.Registers.A);
    }

    [Fact]
    public void ADC_INY_PageCross()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.Y = 0x04;
        target.Bus[0x0010] = 0xFF;
        target.Bus[0x0011] = 0x11;
        target.Bus[0x1203] = 0x11;
        target.Bus[0x8000] = 0x71;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 6);

        Assert.Equal(0xB6, target.Registers.A);
    }

    #endregion

    #region AND

    [Fact]
    public void AND_IMM()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0x29;
        target.Bus[0x8001] = 0x0F;

        ExecuteCycles(target, 2);

        Assert.Equal(0x05, target.Registers.A);
    }

    [Fact]
    public void AND_ZPG()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x0010] = 0x0F;
        target.Bus[0x8000] = 0x25;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 3);

        Assert.Equal(0x05, target.Registers.A);
    }

    [Fact]
    public void AND_ZPX()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.X = 0x04;
        target.Bus[0x0014] = 0x0F;
        target.Bus[0x8000] = 0x35;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 4);

        Assert.Equal(0x05, target.Registers.A);
    }

    [Fact]
    public void AND_ABS()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x1234] = 0x0F;
        target.Bus[0x8000] = 0x2D;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 4);

        Assert.Equal(0x05, target.Registers.A);
    }

    [Fact]
    public void AND_ABX()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.X = 0x04;
        target.Bus[0x1238] = 0x0F;
        target.Bus[0x8000] = 0x3D;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 4);

        Assert.Equal(0x05, target.Registers.A);
    }

    [Fact]
    public void AND_ABX_PageCross()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.X = 0x04;
        target.Bus[0x1203] = 0x0F;
        target.Bus[0x8000] = 0x3D;
        target.Bus[0x8001] = 0xFF;
        target.Bus[0x8002] = 0x11;

        ExecuteCycles(target, 5);

        Assert.Equal(0x05, target.Registers.A);
    }

    [Fact]
    public void AND_ABY()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.Y = 0x04;
        target.Bus[0x1238] = 0x0F;
        target.Bus[0x8000] = 0x39;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 4);

        Assert.Equal(0x05, target.Registers.A);
    }

    [Fact]
    public void AND_ABY_PageCross()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.Y = 0x04;
        target.Bus[0x1203] = 0x0F;
        target.Bus[0x8000] = 0x39;
        target.Bus[0x8001] = 0xFF;
        target.Bus[0x8002] = 0x11;

        ExecuteCycles(target, 5);

        Assert.Equal(0x05, target.Registers.A);
    }

    [Fact]
    public void AND_INX()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.X = 0x04;
        target.Bus[0x0014] = 0x34;
        target.Bus[0x0015] = 0x12;
        target.Bus[0x1234] = 0x0F;
        target.Bus[0x8000] = 0x21;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 6);

        Assert.Equal(0x05, target.Registers.A);
    }

    [Fact]
    public void AND_INY()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.Y = 0x04;
        target.Bus[0x0010] = 0x34;
        target.Bus[0x0011] = 0x12;
        target.Bus[0x1238] = 0x0F;
        target.Bus[0x8000] = 0x31;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 5);

        Assert.Equal(0x05, target.Registers.A);
    }

    [Fact]
    public void AND_INY_PageCross()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.Y = 0x04;
        target.Bus[0x0010] = 0xFF;
        target.Bus[0x0011] = 0x11;
        target.Bus[0x1203] = 0x0F;
        target.Bus[0x8000] = 0x31;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 6);

        Assert.Equal(0x05, target.Registers.A);
    }

    #endregion

    #region ASL

    [Fact]
    public void ASL_IMP()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0x0A;

        ExecuteCycles(target, 2);

        Assert.Equal(0x4A, target.Registers.A);
    }

    [Fact]
    public void ASL_ZPG()
    {
        var target = GetTarget();
        target.Bus[0x0010] = 0xA5;
        target.Bus[0x8000] = 0x06;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 5);

        Assert.Equal(0x4A, target.Bus[0x0010]);
    }

    [Fact]
    public void ASL_ZPX()
    {
        var target = GetTarget();
        target.Registers.X = 0x04;
        target.Bus[0x0014] = 0xA5;
        target.Bus[0x8000] = 0x16;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 6);

        Assert.Equal(0x4A, target.Bus[0x0014]);
    }

    [Fact]
    public void ASL_ABS()
    {
        var target = GetTarget();
        target.Bus[0x1234] = 0xA5;
        target.Bus[0x8000] = 0x0E;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 6);

        Assert.Equal(0x4A, target.Bus[0x1234]);
    }

    [Fact]
    public void ASL_ABX()
    {
        var target = GetTarget();
        target.Registers.X = 0x04;
        target.Bus[0x1238] = 0xA5;
        target.Bus[0x8000] = 0x1E;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 7);

        Assert.Equal(0x4A, target.Bus[0x1238]);
    }

    #endregion

    #region BCC

    [Fact]
    public void BCC_REL_NoBranch()
    {
        var target = GetTarget();
        target.Registers.C = true;
        target.Bus[0x8000] = 0x90;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 2);

        Assert.Equal(0x8002, target.Registers.PC);
    }

    [Fact]
    public void BCC_REL_Branch()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x90;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact]
    public void BCC_REL_PageCross()
    {
        var target = GetTarget();
        target.Registers.PC = 0x80F0;
        target.Bus[0x80F0] = 0x90;
        target.Bus[0x80F1] = 0x10;

        ExecuteCycles(target, 4);

        Assert.Equal(0x8102, target.Registers.PC);
    }

    [Fact]
    public void BCC_REL_Negative()
    {
        const sbyte offset = -16;
        var target = GetTarget();
        target.Bus[0x8000] = 0x90;
        target.Bus[0x8001] = offset.ToByte();

        ExecuteCycles(target, 4);

        Assert.Equal(0x7FF2, target.Registers.PC);
    }

    #endregion

    #region BCS

    [Fact]
    public void BCS_REL_NoBranch()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0xB0;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 2);

        Assert.Equal(0x8002, target.Registers.PC);
    }

    [Fact]
    public void BCS_REL_Branch()
    {
        var target = GetTarget();
        target.Registers.C = true;
        target.Bus[0x8000] = 0xB0;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact]
    public void BCS_REL_PageCross()
    {
        var target = GetTarget();
        target.Registers.C = true;
        target.Registers.PC = 0x80F0;
        target.Bus[0x80F0] = 0xB0;
        target.Bus[0x80F1] = 0x10;

        ExecuteCycles(target, 4);

        Assert.Equal(0x8102, target.Registers.PC);
    }

    [Fact]
    public void BCS_REL_Negative()
    {
        const sbyte offset = -16;
        var target = GetTarget();
        target.Registers.C = true;
        target.Bus[0x8000] = 0xB0;
        target.Bus[0x8001] = offset.ToByte();

        ExecuteCycles(target, 4);

        Assert.Equal(0x7FF2, target.Registers.PC);
    }

    #endregion

    #region BEQ

    [Fact]
    public void BEQ_REL_NoBranch()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0xF0;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 2);

        Assert.Equal(0x8002, target.Registers.PC);
    }

    [Fact]
    public void BEQ_REL_Branch()
    {
        var target = GetTarget();
        target.Registers.Z = true;
        target.Bus[0x8000] = 0xF0;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact]
    public void BEQ_REL_PageCross()
    {
        var target = GetTarget();
        target.Registers.Z = true;
        target.Registers.PC = 0x80F0;
        target.Bus[0x80F0] = 0xF0;
        target.Bus[0x80F1] = 0x10;

        ExecuteCycles(target, 4);

        Assert.Equal(0x8102, target.Registers.PC);
    }

    [Fact]
    public void BEQ_REL_Negative()
    {
        const sbyte offset = -16;
        var target = GetTarget();
        target.Registers.Z = true;
        target.Bus[0x8000] = 0xF0;
        target.Bus[0x8001] = offset.ToByte();

        ExecuteCycles(target, 4);

        Assert.Equal(0x7FF2, target.Registers.PC);
    }

    #endregion

    #region BIT

    [Fact]
    public void BIT_ZPG()
    {
        var target = GetTarget();
        target.Registers.A = 0xFF;
        target.Bus[0x0010] = 0x80;
        target.Bus[0x8000] = 0x24;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 3);

        Assert.False(target.Registers.Z);
        Assert.False(target.Registers.V);
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void BIT_ABS()
    {
        var target = GetTarget();
        target.Registers.A = 0xFF;
        target.Bus[0x1234] = 0x80;
        target.Bus[0x8000] = 0x2C;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 4);

        Assert.False(target.Registers.Z);
        Assert.False(target.Registers.V);
        Assert.True(target.Registers.N);
    }

    #endregion

    #region BMI

    [Fact]
    public void BMI_REL()
    {
        var target = GetTarget();
        target.Registers.N = true;
        target.Bus[0x8000] = 0x30;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact]
    public void BMI_REL_NoBranch()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x30;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 2);

        Assert.Equal(0x8002, target.Registers.PC);
    }

    [Fact]
    public void BMI_REL_Branch()
    {
        var target = GetTarget();
        target.Registers.N = true;
        target.Bus[0x8000] = 0x30;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact]
    public void BMI_REL_PageCross()
    {
        var target = GetTarget();
        target.Registers.N = true;
        target.Registers.PC = 0x80F0;
        target.Bus[0x80F0] = 0x30;
        target.Bus[0x80F1] = 0x10;

        ExecuteCycles(target, 4);

        Assert.Equal(0x8102, target.Registers.PC);
    }

    [Fact]
    public void BMI_REL_Negative()
    {
        const sbyte offset = -16;
        var target = GetTarget();
        target.Registers.N = true;
        target.Bus[0x8000] = 0x30;
        target.Bus[0x8001] = offset.ToByte();

        ExecuteCycles(target, 4);

        Assert.Equal(0x7FF2, target.Registers.PC);
    }

    #endregion BMI

    #region BNE

    [Fact]
    public void BNE_REL_NoBranch()
    {
        var target = GetTarget();
        target.Registers.Z = true;
        target.Bus[0x8000] = 0xD0;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 2);

        Assert.Equal(0x8002, target.Registers.PC);
    }

    [Fact]
    public void BNE_REL_Branch()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0xD0;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact]
    public void BNE_REL_PageCross()
    {
        var target = GetTarget();
        target.Registers.PC = 0x80F0;
        target.Bus[0x80F0] = 0xD0;
        target.Bus[0x80F1] = 0x10;

        ExecuteCycles(target, 4);

        Assert.Equal(0x8102, target.Registers.PC);
    }

    [Fact]
    public void BNE_REL_Negative()
    {
        const sbyte offset = -16;
        var target = GetTarget();
        target.Bus[0x8000] = 0xD0;
        target.Bus[0x8001] = offset.ToByte();

        ExecuteCycles(target, 4);

        Assert.Equal(0x7FF2, target.Registers.PC);
    }

    #endregion

    #region BPL

    [Fact]
    public void BPL_REL_NoBranch()
    {
        var target = GetTarget();
        target.Registers.N = true;
        target.Bus[0x8000] = 0x10;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 2);

        Assert.Equal(0x8002, target.Registers.PC);
    }

    [Fact]
    public void BPL_REL_Branch()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x10;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact]
    public void BPL_REL_PageCross()
    {
        var target = GetTarget();
        target.Registers.PC = 0x80F0;
        target.Bus[0x80F0] = 0x10;
        target.Bus[0x80F1] = 0x10;

        ExecuteCycles(target, 4);

        Assert.Equal(0x8102, target.Registers.PC);
    }

    [Fact]
    public void BPL_REL_Negative()
    {
        const sbyte offset = -16;
        var target = GetTarget();
        target.Bus[0x8000] = 0x10;
        target.Bus[0x8001] = offset.ToByte();

        ExecuteCycles(target, 4);

        Assert.Equal(0x7FF2, target.Registers.PC);
    }

    #endregion

    #region BRK

    [Fact]
    public void BRK_IMP()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x00;
        target.Bus[0xFFFE] = 0x34;
        target.Bus[0xFFFF] = 0x12;

        ExecuteCycles(target, 7);

        Assert.Equal(0xFC, target.Registers.SP);
        Assert.Equal(0x1234, target.Registers.PC);
    }

    #endregion

    #region BVC

    [Fact]
    public void BVC_REL_NoBranch()
    {
        var target = GetTarget();
        target.Registers.V = true;
        target.Bus[0x8000] = 0x50;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 2);

        Assert.Equal(0x8002, target.Registers.PC);
    }

    [Fact]
    public void BVC_REL_Branch()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x50;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact]
    public void BVC_REL_PageCross()
    {
        var target = GetTarget();
        target.Registers.PC = 0x80F0;
        target.Bus[0x80F0] = 0x50;
        target.Bus[0x80F1] = 0x10;

        ExecuteCycles(target, 4);

        Assert.Equal(0x8102, target.Registers.PC);
    }

    [Fact]
    public void BVC_REL_Negative()
    {
        const sbyte offset = -16;
        var target = GetTarget();
        target.Bus[0x8000] = 0x50;
        target.Bus[0x8001] = offset.ToByte();

        ExecuteCycles(target, 4);

        Assert.Equal(0x7FF2, target.Registers.PC);
    }

    #endregion

    #region BVS

    [Fact]
    public void BVS_REL_NoBranch()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x70;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 2);

        Assert.Equal(0x8002, target.Registers.PC);
    }

    [Fact]
    public void BVS_REL_Branch()
    {
        var target = GetTarget();
        target.Registers.V = true;
        target.Bus[0x8000] = 0x70;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact]
    public void BVS_REL_PageCross()
    {
        var target = GetTarget();
        target.Registers.V = true;
        target.Registers.PC = 0x80F0;
        target.Bus[0x80F0] = 0x70;
        target.Bus[0x80F1] = 0x10;

        ExecuteCycles(target, 4);

        Assert.Equal(0x8102, target.Registers.PC);
    }

    [Fact]
    public void BVS_REL_Negative()
    {
        const sbyte offset = -16;
        var target = GetTarget();
        target.Registers.V = true;
        target.Bus[0x8000] = 0x70;
        target.Bus[0x8001] = offset.ToByte();

        ExecuteCycles(target, 4);

        Assert.Equal(0x7FF2, target.Registers.PC);
    }

    #endregion

    #region CLC

    [Fact]
    public void CLC_IMP()
    {
        var target = GetTarget();
        target.Registers.C = true;
        target.Bus[0x8000] = 0x18;

        ExecuteCycles(target, 2);

        Assert.False(target.Registers.C);
    }

    #endregion

    #region CLD

    [Fact]
    public void CLD_IMP()
    {
        var target = GetTarget();
        target.Registers.D = true;
        target.Bus[0x8000] = 0xD8;

        ExecuteCycles(target, 2);

        Assert.False(target.Registers.D);
    }

    #endregion

    #region CLI

    [Fact]
    public void CLI_IMP()
    {
        var target = GetTarget();
        target.Registers.I = true;
        target.Bus[0x8000] = 0x58;

        ExecuteCycles(target, 2);

        Assert.False(target.Registers.I);
    }

    #endregion

    #region CLV

    [Fact]
    public void CLV_IMP()
    {
        var target = GetTarget();
        target.Registers.V = true;
        target.Bus[0x8000] = 0xB8;

        ExecuteCycles(target, 2);

        Assert.False(target.Registers.V);
    }

    #endregion

    #region CMP

    [Fact]
    public void CMP_IMM()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0xC9;
        target.Bus[0x8001] = 0xA5;

        ExecuteCycles(target, 2);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void CMP_ZPG()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x0010] = 0xA5;
        target.Bus[0x8000] = 0xC5;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 3);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void CMP_ZPX()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.X = 0x04;
        target.Bus[0x0014] = 0xA5;
        target.Bus[0x8000] = 0xD5;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 4);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void CMP_ABS()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x1234] = 0xA5;
        target.Bus[0x8000] = 0xCD;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 4);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void CMP_ABX()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.X = 0x04;
        target.Bus[0x1238] = 0xA5;
        target.Bus[0x8000] = 0xDD;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 4);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void CMP_ABX_PageCross()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.X = 0x04;
        target.Bus[0x1203] = 0xA5;
        target.Bus[0x8000] = 0xDD;
        target.Bus[0x8001] = 0xFF;
        target.Bus[0x8002] = 0x11;

        ExecuteCycles(target, 5);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void CMP_ABY()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.Y = 0x04;
        target.Bus[0x1238] = 0xA5;
        target.Bus[0x8000] = 0xD9;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 4);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void CMP_ABY_PageCross()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.Y = 0x04;
        target.Bus[0x1203] = 0xA5;
        target.Bus[0x8000] = 0xD9;
        target.Bus[0x8001] = 0xFF;
        target.Bus[0x8002] = 0x11;

        ExecuteCycles(target, 5);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void CMP_INX()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.X = 0x04;
        target.Bus[0x0014] = 0x34;
        target.Bus[0x0015] = 0x12;
        target.Bus[0x1234] = 0xA5;
        target.Bus[0x8000] = 0xC1;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 6);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }


    [Fact]
    public void CMP_INY()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.Y = 0x04;
        target.Bus[0x0010] = 0x34;
        target.Bus[0x0011] = 0x12;
        target.Bus[0x1238] = 0xA5;
        target.Bus[0x8000] = 0xD1;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 5);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void CMP_INY_PageCross()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.Y = 0x04;
        target.Bus[0x0010] = 0xFF;
        target.Bus[0x0011] = 0x11;
        target.Bus[0x1203] = 0xA5;
        target.Bus[0x8000] = 0xD1;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 6);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    #endregion

    #region CPX

    [Fact]
    public void CPX_IMM()
    {
        var target = GetTarget();
        target.Registers.X = 0xA5;
        target.Bus[0x8000] = 0xE0;
        target.Bus[0x8001] = 0xA5;

        ExecuteCycles(target, 2);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void CPX_ZPG()
    {
        var target = GetTarget();
        target.Registers.X = 0xA5;
        target.Bus[0x0010] = 0xA5;
        target.Bus[0x8000] = 0xE4;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 3);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void CPX_ABS()
    {
        var target = GetTarget();
        target.Registers.X = 0xA5;
        target.Bus[0x1234] = 0xA5;
        target.Bus[0x8000] = 0xEC;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 4);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    #endregion

    #region CPY

    [Fact]
    public void CPY_IMM()
    {
        var target = GetTarget();
        target.Registers.Y = 0xA5;
        target.Bus[0x8000] = 0xC0;
        target.Bus[0x8001] = 0xA5;

        ExecuteCycles(target, 2);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void CPY_ZPG()
    {
        var target = GetTarget();
        target.Registers.Y = 0xA5;
        target.Bus[0x0010] = 0xA5;
        target.Bus[0x8000] = 0xC4;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 3);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void CPY_ABS()
    {
        var target = GetTarget();
        target.Registers.Y = 0xA5;
        target.Bus[0x1234] = 0xA5;
        target.Bus[0x8000] = 0xCC;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 4);

        Assert.True(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    #endregion

    [Fact]
    public void DEC_ZPG()
    {
        var target = GetTarget();
        target.Bus[0x0010] = 0xA5;
        target.Bus[0x8000] = 0xC6;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 5);

        Assert.Equal(0xA4, target.Bus[0x0010]);
    }

    [Fact]
    public void DEX_IMP()
    {
        var target = GetTarget();
        target.Registers.X = 0xA5;
        target.Bus[0x8000] = 0xCA;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA4, target.Registers.X);
        Assert.False(target.Registers.Z);
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void DEY_IMP()
    {
        var target = GetTarget();
        target.Registers.Y = 0xA5;
        target.Bus[0x8000] = 0x88;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA4, target.Registers.Y);
        Assert.False(target.Registers.Z);
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void EOR_IMM()
    {
        var target = GetTarget();
        target.Registers.A = 0x0F;
        target.Bus[0x8000] = 0x49;
        target.Bus[0x8001] = 0x33;

        ExecuteCycles(target, 2);

        Assert.Equal(0x3C, target.Registers.A);
        Assert.False(target.Registers.Z);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void INC_ZPG()
    {
        var target = GetTarget();
        target.Bus[0x0010] = 0xA5;
        target.Bus[0x8000] = 0xE6;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 5);

        Assert.Equal(0xA6, target.Bus[0x0010]);
    }

    [Fact]
    public void INX_IMP()
    {
        var target = GetTarget();
        target.Registers.X = 0xA5;
        target.Bus[0x8000] = 0xE8;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA6, target.Registers.X);
    }

    [Fact]
    public void INY_IMP()
    {
        var target = GetTarget();
        target.Registers.Y = 0xA5;
        target.Bus[0x8000] = 0xC8;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA6, target.Registers.Y);
    }

    [Fact]
    public void JMP_ABS()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x4C;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 3);

        Assert.Equal(0x1234, target.Registers.PC);
    }

    [Fact]
    public void JSR_ABS()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x20;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 6);

        Assert.Equal(0xFD, target.Registers.SP);
        Assert.Equal(0x1234, target.Registers.PC);
    }

    [Fact]
    public void LDA_IMM()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0xA9;
        target.Bus[0x8001] = 0xA5;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA5, target.Registers.A);
    }

    [Fact]
    public void LDX_IMM()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0xA2;
        target.Bus[0x8001] = 0xA5;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA5, target.Registers.X);
    }

    [Fact]
    public void LDY_IMM()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0xA0;
        target.Bus[0x8001] = 0xA5;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA5, target.Registers.Y);
    }

    [Fact]
    public void LSR_IMP()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0x4A;

        ExecuteCycles(target, 2);

        Assert.Equal(0x52, target.Registers.A);
    }

    [Fact]
    public void LSR_ZPG()
    {
        var target = GetTarget();
        target.Bus[0x0010] = 0xA5;
        target.Bus[0x8000] = 0x46;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 5);

        Assert.Equal(0x52, target.Bus[0x0010]);
    }

    [Fact]
    public void NOP_IMP()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0xEA;

        ExecuteCycles(target, 2);
    }

    [Fact]
    public void ORA_IMM()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0x09;
        target.Bus[0x8001] = 0x0F;

        ExecuteCycles(target, 2);

        Assert.Equal(0xAF, target.Registers.A);
    }

    [Fact]
    public void PHA_IMP()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0x48;

        ExecuteCycles(target, 3);

        Assert.Equal(0xFE, target.Registers.SP);
        Assert.Equal(0xA5, target.Bus[0x01FF]);
    }

    [Fact]
    public void PHP_IMP()
    {
        var target = GetTarget();
        target.Registers.C = true;
        target.Registers.I = true;
        target.Registers.N = true;
        target.Bus[0x8000] = 0x08;

        ExecuteCycles(target, 3);

        Assert.Equal(0xFE, target.Registers.SP);
        Assert.Equal(0xB5, target.Bus[0x01FF]);
    }

    [Fact]
    public void PLA_IMP()
    {
        var target = GetTarget();
        target.Registers.SP = 0xFE;
        target.Bus[0x01FE] = 0xA5;
        target.Bus[0x8000] = 0x68;

        ExecuteCycles(target, 4);

        Assert.Equal(0xFF, target.Registers.SP);
        Assert.Equal(0xA5, target.Registers.A);
    }

    [Fact]
    public void PLP_IMP()
    {
        var target = GetTarget();
        target.Registers.SP = 0xFE;
        target.Bus[0x01FE] = 0xA5;
        target.Bus[0x8000] = 0x28;

        ExecuteCycles(target, 4);

        Assert.Equal(0xFF, target.Registers.SP);
        Assert.True(target.Registers.C);
        Assert.False(target.Registers.Z);
        Assert.True(target.Registers.I);
        Assert.False(target.Registers.D);
        Assert.False(target.Registers.V);
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void ROL_IMP()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0x2A;

        ExecuteCycles(target, 2);

        Assert.Equal(0x4A, target.Registers.A);
    }

    [Fact]
    public void ROL_ZPG()
    {
        var target = GetTarget();
        target.Bus[0x0010] = 0xA5;
        target.Bus[0x8000] = 0x26;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 5);

        Assert.Equal(0x4A, target.Bus[0x0010]);
    }

    [Fact]
    public void ROR_IMP()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0x6A;

        ExecuteCycles(target, 2);

        Assert.Equal(0x52, target.Registers.A);
    }

    [Fact]
    public void ROR_ZPG()
    {
        var target = GetTarget();
        target.Bus[0x0010] = 0xA5;
        target.Bus[0x8000] = 0x66;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 5);

        Assert.Equal(0x52, target.Bus[0x0010]);
    }

    [Fact]
    public void RTI_IMP()
    {
        var target = GetTarget();
        target.Registers.SP = 0xFD;
        target.Bus[0x01FD] = 0xB5;
        target.Bus[0x01FE] = 0x12;
        target.Bus[0x01FF] = 0x34;
        target.Bus[0x8000] = 0x40;

        ExecuteCycles(target, 6);

        Assert.Equal(0x1234, target.Registers.PC);
        Assert.True(target.Registers.C);
        Assert.True(target.Registers.I);
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void RTS_IMP()
    {
        var target = GetTarget();
        target.Registers.SP = 0xFE;
        target.Bus[0x01FE] = 0x12;
        target.Bus[0x01FF] = 0x34;
        target.Bus[0x8000] = 0x60;

        ExecuteCycles(target, 6);

        Assert.Equal(0x1233, target.Registers.PC);
    }

    [Fact]
    public void SBC_IMM()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Registers.C = true;
        target.Bus[0x8000] = 0xE9;
        target.Bus[0x8001] = 0x11;

        ExecuteCycles(target, 2);

        Assert.Equal(0x94, target.Registers.A);
    }

    [Fact]
    public void SEC_IMP()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x38;

        ExecuteCycles(target, 2);

        Assert.True(target.Registers.C);
    }

    [Fact]
    public void SED_IMP()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0xF8;

        ExecuteCycles(target, 2);

        Assert.True(target.Registers.D);
    }

    [Fact]
    public void SEI_IMP()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x78;

        ExecuteCycles(target, 2);

        Assert.True(target.Registers.I);
    }

    [Fact]
    public void STA_ZPG()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0x85;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 3);

        Assert.Equal(0xA5, target.Bus[0x0010]);
    }

    [Fact]
    public void STX_ZPG()
    {
        var target = GetTarget();
        target.Registers.X = 0xA5;
        target.Bus[0x8000] = 0x86;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 3);

        Assert.Equal(0xA5, target.Bus[0x0010]);
    }

    [Fact]
    public void STY_ZPG()
    {
        var target = GetTarget();
        target.Registers.Y = 0xA5;
        target.Bus[0x8000] = 0x84;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 3);

        Assert.Equal(0xA5, target.Bus[0x0010]);
    }

    [Fact]
    public void TAX_IMP()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0xAA;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA5, target.Registers.X);
    }

    [Fact]
    public void TAY_IMP()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0xA8;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA5, target.Registers.Y);
    }

    [Fact]
    public void TSX_IMP()
    {
        var target = GetTarget();
        target.Registers.SP = 0xA5;
        target.Bus[0x8000] = 0xBA;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA5, target.Registers.X);
    }

    [Fact]
    public void TXA_IMP()
    {
        var target = GetTarget();
        target.Registers.X = 0xA5;
        target.Bus[0x8000] = 0x8A;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA5, target.Registers.A);
    }

    [Fact]
    public void TXS_IMP()
    {
        var target = GetTarget();
        target.Registers.X = 0xA5;
        target.Bus[0x8000] = 0x9A;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA5, target.Registers.SP);
    }

    [Fact]
    public void TYA_IMP()
    {
        var target = GetTarget();
        target.Registers.Y = 0xA5;
        target.Bus[0x8000] = 0x98;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA5, target.Registers.A);
    }

    private static void ExecuteCycles(Processor target, int cycles)
    {
        const int maxCycles = 20;
        int currCycles = 0;
        do
        {
            target.ExecuteClock();
            currCycles++;
            if (!target.IsExecuting) break;
        } while (currCycles < maxCycles);

        if (currCycles != cycles)
            throw new Xunit.Sdk.XunitException($"Expected {cycles} cycles, took {currCycles} cycles");
    }

    private static Processor GetTarget()
    {
        var target = new Processor(new SimpleBus());
        target.Registers.SP = 0xFF;
        target.Registers.PC = 0x8000;
        return target;
    }
}
