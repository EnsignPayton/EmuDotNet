using EmuDotNet.Core.Abstractions;
using EmuDotNet.Core.MC6800;
using Xunit;

namespace EmuDotNet.Core.Test.MC6800
{
    public class ProcessorTests
    {
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
                (byte)Instruction.CLR_A);

            target.Registers.N = true;
            target.ExecuteClock();

            Assert.Equal(0x00, target.Registers.A);
            Assert.False(target.Registers.N);
        }

        [Fact]
        public void Clear_Sets_Zero_Flag()
        {
            var target = GetTestTarget(
                (byte)Instruction.CLR_A);

            target.Registers.Z = false;
            target.ExecuteClock();

            Assert.Equal(0x00, target.Registers.A);
            Assert.True(target.Registers.Z);
        }

        [Fact]
        public void Clear_Clears_Carry_Flag()
        {
            var target = GetTestTarget(
                (byte)Instruction.CLR_A);

            target.Registers.C = true;
            target.ExecuteClock();

            Assert.Equal(0x00, target.Registers.A);
            Assert.False(target.Registers.C);
        }

        [Fact]
        public void Clear_Clears_Overflow_Flag()
        {
            var target = GetTestTarget(
                (byte)Instruction.CLR_A);

            target.Registers.V = true;
            target.ExecuteClock();

            Assert.Equal(0x00, target.Registers.A);
            Assert.False(target.Registers.V);
        }

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
}
