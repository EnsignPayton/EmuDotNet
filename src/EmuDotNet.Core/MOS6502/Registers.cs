namespace EmuDotNet.Core.MOS6502;

public class Registers
{
    public byte A { get; set; }
    public byte X { get; set; }
    public byte Y { get; set; }
    public byte SP { get; set; } = 0xFD;
    public ushort PC { get; set; }

    // Status flags
    public bool C { get; set; }
    public bool Z { get; set; }
    public bool I { get; set; } = true;
    public bool D { get; set; }
    public bool V { get; set; }
    public bool N { get; set; }
}