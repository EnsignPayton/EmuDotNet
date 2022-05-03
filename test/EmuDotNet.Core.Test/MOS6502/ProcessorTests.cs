using System.Runtime.InteropServices;

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
    public void ASL_IMP()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0x0A;

        ExecuteCycles(target, 2);

        Assert.Equal(0x4A, target.Registers.A);
    }

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
        target.Bus[0x8001] = Cast(offset);

        ExecuteCycles(target, 4);

        Assert.Equal(0x7FF2, target.Registers.PC);
    }

    [Fact]
    public void BCS_REL()
    {
        var target = GetTarget();
        target.Registers.C = true;
        target.Bus[0x8000] = 0xB0;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact]
    public void BEQ_REL()
    {
        var target = GetTarget();
        target.Registers.Z = true;
        target.Bus[0x8000] = 0xF0;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

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
    public void BNE_REL()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0xD0;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact]
    public void BPL_REL()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x10;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact(Skip = "TODO: Stack")]
    public void BRK_IMP()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x00;

        ExecuteCycles(target, 7);

        Assert.Equal(0xFFFE, target.Registers.PC);
    }

    [Fact]
    public void BVC_REL()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x50;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact]
    public void BVS_REL()
    {
        var target = GetTarget();
        target.Registers.V = true;
        target.Bus[0x8000] = 0x70;
        target.Bus[0x8001] = 0x04;

        ExecuteCycles(target, 3);

        Assert.Equal(0x8006, target.Registers.PC);
    }

    [Fact]
    public void CLC_IMP()
    {
        var target = GetTarget();
        target.Registers.C = true;
        target.Bus[0x8000] = 0x18;

        ExecuteCycles(target, 2);

        Assert.False(target.Registers.C);
    }

    [Fact]
    public void CLD_IMP()
    {
        var target = GetTarget();
        target.Registers.D = true;
        target.Bus[0x8000] = 0xD8;

        ExecuteCycles(target, 2);

        Assert.False(target.Registers.D);
    }

    [Fact]
    public void CLI_IMP()
    {
        var target = GetTarget();
        target.Registers.I = true;
        target.Bus[0x8000] = 0x58;

        ExecuteCycles(target, 2);

        Assert.False(target.Registers.I);
    }

    [Fact]
    public void CLV_IMP()
    {
        var target = GetTarget();
        target.Registers.V = true;
        target.Bus[0x8000] = 0xB8;

        ExecuteCycles(target, 2);

        Assert.False(target.Registers.V);
    }

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

    [Fact(Skip = "TODO: Store")]
    public void DEC_ZPG()
    {
        var target = GetTarget();
        target.Bus[0x0010] = 0xA5;
        target.Bus[0x8000] = 0xC6;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 5);

        Assert.Equal(0xA4, target.Bus[0x0010]);
    }

    // TODO: In Order, etc

    [Fact]
    public void LDA_Loads_Immediate()
    {
        var target = GetTarget();
        target.Registers.PC = 0x8000;
        target.Bus[0x8000] = 0xA9;
        target.Bus[0x8001] = 0xA5;

        ExecuteCycles(target, 2);

        Assert.Equal(0xA5, target.Registers.A);
    }

    private static void ExecuteCycles(Processor target, int cycles)
    {
        target.ExecuteClock();

        for (int i = 1; i < cycles; i++)
        {
            Assert.True(target.IsExecuting);
            target.ExecuteClock();
        }

        Assert.False(target.IsExecuting);
    }

    private static Processor GetTarget()
    {
        var target = new Processor(new SimpleBus());
        target.Registers.PC = 0x8000;
        return target;
    }

    private static byte Cast(sbyte value) =>
        MemoryMarshal.Cast<sbyte, byte>(stackalloc sbyte[] {value})[0];

    private static sbyte Cast(byte value) =>
        MemoryMarshal.Cast<byte, sbyte>(stackalloc byte[] {value})[0];
}
