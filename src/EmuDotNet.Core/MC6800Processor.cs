namespace EmuDotNet.Core
{
    public class MC6800Processor : IProcessor
    {
        private readonly IMemory _memory;
        private readonly MC6800Registers _registers = new();

        public MC6800Processor(
            IMemory memory)
        {
            _memory = memory;
        }

        public void ExecuteClock()
        {
        }
    }
}
