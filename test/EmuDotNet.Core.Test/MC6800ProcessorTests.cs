using System;
using Xunit;

namespace EmuDotNet.Core.Test
{
    public class MC6800ProcessorTests
    {
        [Fact]
        public void ABA_Zeros()
        {
            var target = GetTestTarget(new[]
            {
                (byte) MC6800Instruction.ABA
            });

            target.ExecuteClock();

            Assert.Equal(0, target.Registers.A);
        }

        [Fact]
        public void ADD_A_IMM_Loads()
        {
            var target = GetTestTarget(new[]
            {
                (byte) MC6800Instruction.ADD_A_IMM,
                (byte) 0x69
            });

            target.ExecuteClock();

            Assert.Equal(0x69, target.Registers.A);
        }

        private static MC6800Processor GetTestTarget(ReadOnlySpan<byte> data) =>
            new MC6800Processor(new SimpleMemory(data));
    }
}
