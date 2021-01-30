using System;
using EmuDotNet.Core.MC6800;
using Xunit;

namespace EmuDotNet.Core.Test.MC6800
{
    public class ProcessorTests
    {
        [Fact]
        public void ABA_Zeros()
        {
            var target = GetTestTarget(new[]
            {
                (byte) Instruction.ABA
            });

            target.ExecuteClock();

            Assert.Equal(0, target.Registers.A);
        }

        [Fact]
        public void ADD_A_IMM_Loads()
        {
            var target = GetTestTarget(new[]
            {
                (byte) Instruction.ADD_A_IMM,
                (byte) 0x69
            });

            target.ExecuteClock();

            Assert.Equal(0x69, target.Registers.A);
        }

        private static Processor GetTestTarget(ReadOnlySpan<byte> data) =>
            new Processor(new SimpleMemory(data));
    }
}
