namespace EmuDotNet.Core.MC6800;

public class Registers
{
    /// <summary>
    /// Accumulator A
    /// </summary>
    public byte A { get; set; }

    /// <summary>
    /// Accumulator B
    /// </summary>
    public byte B { get; set; }

    /// <summary>
    /// Index Register
    /// </summary>
    public ushort IX { get; set; }

    /// <summary>
    /// Stack Pointer
    /// </summary>
    public ushort SP { get; set; }

    /// <summary>
    /// Program Counter
    /// </summary>
    public ushort PC { get; set; }

    /// <summary>
    /// Half-Carry Flag
    /// </summary>
    public bool H { get; set; }

    /// <summary>
    /// Interrupt Flag
    /// </summary>
    public bool I { get; set; }

    /// <summary>
    /// Sign Flag
    /// </summary>
    public bool N { get; set; }

    /// <summary>
    /// Zero Flag
    /// </summary>
    public bool Z { get; set; }

    /// <summary>
    /// Overflow Flag
    /// </summary>
    public bool V { get; set; }

    /// <summary>
    /// Carry Flag
    /// </summary>
    public bool C { get; set; }
}
