using EmuDotNet.Core.Abstractions;
using EmuDotNet.Core.MC6800;
using Xunit;

namespace EmuDotNet.Core.Test.MC6800;

public class ProcessorTests
{
    [Fact]
    public void NOP_Only_Increments_Program_Counter()
    {
        var target = GetTestTarget(Instruction.NOP);

        target.ExecuteClock();

        Assert.Equal(0x00, target.Registers.A);
        Assert.Equal(0x00, target.Registers.B);
        Assert.Equal(0x0000, target.Registers.IX);
        Assert.Equal(0x0000, target.Registers.SP);
        Assert.Equal(0x0001, target.Registers.PC);
        Assert.False(target.Registers.H);
        Assert.False(target.Registers.I);
        Assert.False(target.Registers.N);
        Assert.False(target.Registers.Z);
        Assert.False(target.Registers.V);
        Assert.False(target.Registers.C);
    }

    // TODO: Find definition for TAP and TPA

    [Fact]
    public void INX_Increments_Index_Register()
    {
        var target = GetTestTarget(Instruction.INX);

        target.ExecuteClock();

        Assert.Equal(0x0001, target.Registers.IX);
    }

    [Fact]
    public void DEX_Decrements_Index_Register()
    {
        var target = GetTestTarget(Instruction.DEX);

        target.ExecuteClock();

        Assert.Equal(0xFFFF, target.Registers.IX);
    }

    [Fact]
    public void CLV_Clears_Overflow_Flag()
    {
        var target = GetTestTarget(Instruction.CLV);

        target.Registers.V = true;
        target.ExecuteClock();

        Assert.False(target.Registers.V);
    }

    [Fact]
    public void SEV_Sets_Overflow_Flag()
    {
        var target = GetTestTarget(Instruction.SEV);

        target.Registers.V = false;
        target.ExecuteClock();

        Assert.True(target.Registers.V);
    }

    [Fact]
    public void CLC_Clears_Overflow_Flag()
    {
        var target = GetTestTarget(Instruction.CLC);

        target.Registers.C = true;
        target.ExecuteClock();

        Assert.False(target.Registers.C);
    }

    [Fact]
    public void SEC_Sets_Overflow_Flag()
    {
        var target = GetTestTarget(Instruction.SEC);

        target.Registers.C = false;
        target.ExecuteClock();

        Assert.True(target.Registers.C);
    }

    [Fact]
    public void CLI_Clears_Overflow_Flag()
    {
        var target = GetTestTarget(Instruction.CLI);

        target.Registers.I = true;
        target.ExecuteClock();

        Assert.False(target.Registers.I);
    }

    [Fact]
    public void SEI_Sets_Overflow_Flag()
    {
        var target = GetTestTarget(Instruction.SEI);

        target.Registers.I = false;
        target.ExecuteClock();

        Assert.True(target.Registers.I);
    }

    // Old tests below

    [Fact]
    public void Add_Sets_Half_Carry()
    {
        var target = GetTestTarget(
            (byte) Instruction.ADD_A_IMM, 0x0F,
            (byte) Instruction.ADD_A_IMM, 0x01,
            (byte) Instruction.ABA);

        target.ExecuteClock(2);

        Assert.Equal(0x10, target.Registers.A);
        Assert.True(target.Registers.H);
    }

    [Fact]
    public void Add_Sets_Sign_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.ADD_A_IMM, 0xFF);

        target.ExecuteClock();

        Assert.Equal(0xFF, target.Registers.A);
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void Add_Sets_Zero_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.ADD_A_IMM, 0x00);

        target.ExecuteClock();

        Assert.Equal(0x00, target.Registers.A);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void Add_Sets_Overflow_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.ADD_A_IMM, 0x7F,
            (byte) Instruction.ADD_A_IMM, 0x01);

        target.ExecuteClock(2);

        Assert.Equal(0x80, target.Registers.A);
        Assert.True(target.Registers.V);
    }

    [Fact]
    public void Add_Sets_Carry_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.ADD_A_IMM, 0x80,
            (byte) Instruction.ADD_A_IMM, 0x80);

        target.ExecuteClock(2);

        Assert.Equal(0x00, target.Registers.A);
        Assert.True(target.Registers.C);
    }

    [Fact]
    public void Add_With_Carry_Includes_Carry()
    {
        var target = GetTestTarget(
            (byte) Instruction.ADD_A_IMM, 0x80,
            (byte) Instruction.ADD_A_IMM, 0x80,
            (byte) Instruction.ADC_A_IMM, 0x01);

        target.ExecuteClock(3);

        Assert.Equal(0x02, target.Registers.A);
    }

    [Fact]
    public void Add_Accumulators_Works()
    {
        var target = GetTestTarget(
            (byte) Instruction.ABA);

        target.Registers.A = 0x0F;
        target.Registers.B = 0x01;
        target.ExecuteClock();

        Assert.Equal(0x10, target.Registers.A);
    }

    [Fact]
    public void Add_Direct_Loads_Data()
    {
        var target = GetTestTarget(
            (byte) Instruction.ADD_A_DIR, 0x04, 0x00, 0x00, 0x69);

        target.ExecuteClock();

        Assert.Equal(0x69, target.Registers.A);
    }

    [Fact]
    public void Add_Extended_Loads_Data()
    {
        var target = GetTestTarget(
            (byte) Instruction.ADD_A_EXT, 0x00, 0x04, 0x00, 0x69);

        target.ExecuteClock();

        Assert.Equal(0x69, target.Registers.A);
    }

    [Fact]
    public void Add_Indexed_Loads_Data()
    {
        var target = GetTestTarget(
            (byte) Instruction.ADD_A_IDX, 0x01, 0x00, 0x00, 0x69);

        target.Registers.IX = 0x03;
        target.ExecuteClock();

        Assert.Equal(0x69, target.Registers.A);
    }

    [Fact]
    public void And_Works()
    {
        var target = GetTestTarget(
            (byte) Instruction.AND_A_IMM, 0xAA);

        target.Registers.A = 0x0F;
        target.ExecuteClock();

        Assert.Equal(0x0A, target.Registers.A);
    }

    [Fact]
    public void And_Sets_Sign_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.AND_A_IMM, 0xAA);

        target.Registers.A = 0xF0;
        target.ExecuteClock();

        Assert.Equal(0xA0, target.Registers.A);
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void And_Sets_Zero_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.AND_A_IMM, 0xAA);

        target.ExecuteClock();

        Assert.Equal(0x00, target.Registers.A);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void And_Clears_Overflow_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.AND_A_IMM, 0xFF);

        target.Registers.A = 0xFF;
        target.ExecuteClock();

        Assert.Equal(0xFF, target.Registers.A);
        Assert.False(target.Registers.V);
    }

    [Fact]
    public void Bit_Sets_Sign_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.BIT_A_IMM, 0xAA);

        target.Registers.A = 0xF0;
        target.ExecuteClock();

        Assert.Equal(0xF0, target.Registers.A);
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void Bit_Sets_Zero_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.BIT_A_IMM, 0xAA);

        target.ExecuteClock();

        Assert.Equal(0x00, target.Registers.A);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void Bit_Clears_Overflow_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.BIT_A_IMM, 0xFF);

        target.Registers.A = 0xFF;
        target.ExecuteClock();

        Assert.Equal(0xFF, target.Registers.A);
        Assert.False(target.Registers.V);
    }

    [Fact]
    public void Clear_Clears_Accumulator()
    {
        var target = GetTestTarget(
            (byte) Instruction.CLR_A);

        target.Registers.A = 0x69;
        target.ExecuteClock();

        Assert.Equal(0x00, target.Registers.A);
    }

    [Fact]
    public void Clear_Clears_Indexed()
    {
        var target = GetTestTarget(
            (byte) Instruction.CLR_IDX, 0x00);

        target.ExecuteClock();

        Assert.Equal(0, target.Data[0]);
    }

    [Fact]
    public void Clear_Clears_Extended()
    {
        var target = GetTestTarget(
            (byte) Instruction.CLR_EXT, 0x00, 0x00);

        target.ExecuteClock();

        Assert.Equal(0, target.Data[0]);
    }

    [Fact]
    public void Clear_Clears_Sign_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.CLR_A);

        target.Registers.N = true;
        target.ExecuteClock();

        Assert.Equal(0x00, target.Registers.A);
        Assert.False(target.Registers.N);
    }

    [Fact]
    public void Clear_Sets_Zero_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.CLR_A);

        target.Registers.Z = false;
        target.ExecuteClock();

        Assert.Equal(0x00, target.Registers.A);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void Clear_Clears_Carry_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.CLR_A);

        target.Registers.C = true;
        target.ExecuteClock();

        Assert.Equal(0x00, target.Registers.A);
        Assert.False(target.Registers.C);
    }

    [Fact]
    public void Clear_Clears_Overflow_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.CLR_A);

        target.Registers.V = true;
        target.ExecuteClock();

        Assert.Equal(0x00, target.Registers.A);
        Assert.False(target.Registers.V);
    }

    [Fact]
    public void Complement_Inverts_Accumulator()
    {
        var target = GetTestTarget(
            (byte) Instruction.COM_A);

        target.Registers.A = 0xAA;
        target.ExecuteClock();

        Assert.Equal(0x55, target.Registers.A);
    }

    [Fact]
    public void Complement_Inverts_Indexed()
    {
        var target = GetTestTarget(
            (byte) Instruction.COM_IDX, 0x02, 0xAA);

        target.ExecuteClock();

        Assert.Equal(0x55, target.Data[2]);
    }

    [Fact]
    public void Complement_Inverts_Extended()
    {
        var target = GetTestTarget(
            (byte) Instruction.COM_EXT, 0x00, 0x03, 0xAA);

        target.ExecuteClock();

        Assert.Equal(0x55, target.Data[3]);
    }

    [Fact]
    public void Complement_Sets_Sign_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.COM_A);

        target.ExecuteClock();

        Assert.Equal(0xFF, target.Registers.A);
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void Complement_Sets_Zero_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.COM_A);

        target.Registers.A = 0xFF;
        target.ExecuteClock();

        Assert.Equal(0x00, target.Registers.A);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void Complement_Clears_Overflow_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.COM_A);

        target.Registers.V = true;
        target.ExecuteClock();

        Assert.Equal(0xFF, target.Registers.A);
        Assert.False(target.Registers.V);
    }

    [Fact]
    public void Complement_Sets_Carry_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.COM_A);

        target.ExecuteClock();

        Assert.Equal(0xFF, target.Registers.A);
        Assert.True(target.Registers.C);
    }

    [Fact]
    public void Negate_Inverts_Accumulator()
    {
        var target = GetTestTarget(
            (byte) Instruction.NEG_A);

        target.Registers.A = 0xAA;
        target.ExecuteClock();

        Assert.Equal(0x56, target.Registers.A);
    }

    [Fact]
    public void Negate_Inverts_Indexed()
    {
        var target = GetTestTarget(
            (byte) Instruction.NEG_IDX, 0x02, 0xAA);

        target.ExecuteClock();

        Assert.Equal(0x56, target.Data[2]);
    }

    [Fact]
    public void Negate_Inverts_Extended()
    {
        var target = GetTestTarget(
            (byte) Instruction.NEG_EXT, 0x00, 0x03, 0xAA);

        target.ExecuteClock();

        Assert.Equal(0x56, target.Data[3]);
    }

    [Fact]
    public void Negate_Sets_Sign_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.NEG_A);

        target.Registers.A = 0x7F;
        target.ExecuteClock();

        Assert.Equal(0x81, target.Registers.A);
        Assert.True(target.Registers.N);
    }

    [Fact]
    public void Negate_Sets_Zero_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.NEG_A);

        target.ExecuteClock();

        Assert.Equal(0x00, target.Registers.A);
        Assert.True(target.Registers.Z);
    }

    [Fact]
    public void Negate_Sets_Overflow_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.NEG_A);

        target.Registers.A = 0x80;
        target.ExecuteClock();

        Assert.Equal(0x80, target.Registers.A);
        Assert.True(target.Registers.V);
    }

    [Fact]
    public void Negate_Sets_Carry_Flag()
    {
        var target = GetTestTarget(
            (byte) Instruction.NEG_A);

        target.ExecuteClock();

        Assert.Equal(0x00, target.Registers.A);
        Assert.True(target.Registers.C);
    }

    private static ProcessorFacade GetTestTarget(Instruction instruction) =>
        new((byte) instruction);

    private static ProcessorFacade GetTestTarget(params byte[] data) =>
        new(data);

    private class ProcessorFacade : IProcessor
    {
        private readonly Processor _processor;
        private readonly SimpleMemory _memory;

        public Registers Registers => _processor.Registers;
        public byte[] Data => _memory.Data;

        public ProcessorFacade(params byte[] data)
        {
            _memory = new SimpleMemory(data);
            _processor = new Processor(_memory);
        }

        public void ExecuteClock()
        {
            _processor.ExecuteClock();
        }

        public void ExecuteClock(int cycles)
        {
            for (int i = 0; i < cycles; i++)
            {
                _processor.ExecuteClock();
            }
        }
    }
}
