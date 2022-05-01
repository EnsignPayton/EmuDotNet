namespace EmuDotNet.Core.MOS6502;

public class ProcessorTests
{
    private static Processor GetTarget() => new(new SimpleBus());
}