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

    [Fact(Skip = "TODO: Store")]
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

    [Fact(Skip = "TODO: Address as operand")]
    public void JMP_ABS()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x4C;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 3);

        Assert.Equal(0x1234, target.Registers.PC);
    }

    [Fact(Skip = "TODO: Address as operand; Stack")]
    public void JSR_ABS()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x20;
        target.Bus[0x8001] = 0x34;
        target.Bus[0x8002] = 0x12;

        ExecuteCycles(target, 6);

        // TODO: Test stack
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

    [Fact(Skip = "TODO: Store")]
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

    [Fact(Skip = "TODO: Stack")]
    public void PHA_IMP()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0x48;

        ExecuteCycles(target, 3);

        // TODO: Test stack
    }

    [Fact(Skip = "TODO: Stack")]
    public void PHP_IMP()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x08;

        ExecuteCycles(target, 3);

        // TODO: Test Stack
    }

    [Fact(Skip = "TODO: Stack")]
    public void PLA_IMP()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x68;

        ExecuteCycles(target, 4);

        // TODO: Test Stack
    }

    [Fact(Skip = "TODO: Stack")]
    public void PLP_IMP()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x28;

        ExecuteCycles(target, 4);

        // TODO: Test Stack
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

    [Fact(Skip = "TODO: Store")]
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

    [Fact(Skip = "TODO: Store")]
    public void ROR_ZPG()
    {
        var target = GetTarget();
        target.Bus[0x0010] = 0xA5;
        target.Bus[0x8000] = 0x66;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 5);

        Assert.Equal(0x52, target.Bus[0x0010]);
    }

    [Fact(Skip = "TODO: Stack")]
    public void RTI_IMP()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x40;

        ExecuteCycles(target, 6);

        // TODO: Test Stack
    }

    [Fact(Skip = "TODO: Stack")]
    public void RTS_IMP()
    {
        var target = GetTarget();
        target.Bus[0x8000] = 0x60;

        ExecuteCycles(target, 6);

        // TODO: Test Stack
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

    [Fact(Skip = "TODO: Store")]
    public void STA_ZPG()
    {
        var target = GetTarget();
        target.Registers.A = 0xA5;
        target.Bus[0x8000] = 0x85;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 3);

        Assert.Equal(0xA5, target.Bus[0x0010]);
    }

    [Fact(Skip = "TODO: Store")]
    public void STX_ZPG()
    {
        var target = GetTarget();
        target.Registers.X = 0xA5;
        target.Bus[0x8000] = 0x86;
        target.Bus[0x8001] = 0x10;

        ExecuteCycles(target, 3);

        Assert.Equal(0xA5, target.Bus[0x0010]);
    }

    [Fact(Skip = "TODO: Store")]
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
