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
    public bool B { get; set; }
    public bool V { get; set; }
    public bool N { get; set; }

    // TODO: Improve me
    public byte P
    {
        get => GetP();
        set => SetP(value);
    }

    private byte GetP() =>
        (byte) ((N ? 0x80 : 0) +
                (V ? 0x40 : 0) +
                0x20 +
                (B ? 0x10 : 0) +
                (D ? 0x08 : 0) +
                (I ? 0x04 : 0) +
                (Z ? 0x02 : 0) +
                (C ? 0x01 : 0));

    private void SetP(byte value)
    {
        N = (value & 0x80) != 0;
        V = (value & 0x40) != 0;
        B = (value & 0x10) != 0;
        D = (value & 0x08) != 0;
        I = (value & 0x04) != 0;
        Z = (value & 0x02) != 0;
        C = (value & 0x01) != 0;
    }
}
