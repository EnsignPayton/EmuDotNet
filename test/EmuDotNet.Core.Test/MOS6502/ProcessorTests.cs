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
}
