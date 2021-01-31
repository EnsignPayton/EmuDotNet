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
                (byte) Instruction.ADD_A_IMM, 0x39,
                (byte) Instruction.ADD_B_IMM, 0x48,
                (byte) Instruction.ABA);

            target.ExecuteClock();
            target.ExecuteClock();
            target.ExecuteClock();

            Assert.Equal(0x81, target.Registers.A);
            Assert.True(target.Registers.H);
        }

        [Fact]
        public void ABA_Zeros()
        {
            var target = GetTestTarget((byte) Instruction.ABA);

            target.ExecuteClock();

            Assert.Equal(0, target.Registers.A);
        }

        [Fact]
        public void ADD_A_IMM_Loads()
        {
            var target = GetTestTarget((byte) Instruction.ADD_A_IMM, 0x69);

            target.ExecuteClock();

            Assert.Equal(0x69, target.Registers.A);
        }

        #endregion

        private static Processor GetTestTarget(params byte[] data) =>
            GetTestTarget(data.AsSpan());

        private static Processor GetTestTarget(ReadOnlySpan<byte> data) =>
            new Processor(new SimpleMemory(data));
    }
}
