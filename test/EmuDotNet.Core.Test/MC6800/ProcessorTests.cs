using System;
using EmuDotNet.Core.MC6800;
using Xunit;

namespace EmuDotNet.Core.Test.MC6800
{
    public class ProcessorTests
    {
        #region Add Instructions

        [Fact]
        public void Add_Sets_Half_Carry()
        {
            var target = GetTestTarget(
                (byte) Instruction.ADD_A_IMM, 0x0F,
                (byte) Instruction.ADD_A_IMM, 0x01,
                (byte) Instruction.ABA);

            target.ExecuteClock();
            target.ExecuteClock();

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

            target.ExecuteClock();
            target.ExecuteClock();

            Assert.Equal(0x80, target.Registers.A);
            Assert.True(target.Registers.V);
        }

        [Fact]
        public void Add_Sets_Carry_Flag()
        {
            var target = GetTestTarget(
                (byte) Instruction.ADD_A_IMM, 0x80,
                (byte) Instruction.ADD_A_IMM, 0x80);

            target.ExecuteClock();
            target.ExecuteClock();

            Assert.Equal(0x00, target.Registers.A);
            Assert.True(target.Registers.C);
        }

        #endregion

        private static Processor GetTestTarget(params byte[] data) =>
            new Processor(new SimpleMemory(data));
    }
}
