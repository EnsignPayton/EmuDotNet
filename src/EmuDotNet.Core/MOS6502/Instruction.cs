namespace EmuDotNet.Core.MOS6502;

public enum Instruction
{
    ADC, AND, ASL, BCC, BCS, BEQ, BIT, BMI,
    BNE, BPL, BRK, BVC, BVS, CLC, CLD, CLI,
    CLV, CMP, CPX, CPY, DEC, DEX, DEY, EOR,
    INC, INX, INY, JMP, JSR, LDA, LDX, LDY,
    LSR, NOP, ORA, PHA, PHP, PLA, PLP, ROL,
    ROR, RTI, RTS, SBC, SEC, SED, SEI, STA,
    STX, STY, TAX, TAY, TSX, TXA, TXS, TYA,
}

public enum AddressMode
{
    ABS, ABX, ABY, IMM, IMP, IND,
    INX, INY, REL, ZPG, ZPX, ZPY,
}

public record Opcode(
    byte Value,
    Instruction Instruction,
    AddressMode AddressMode,
    int Cycles)
{
    private static readonly Opcode[] Table = {
        new(0x69, Instruction.ADC, AddressMode.IMM, 2),
        new(0x65, Instruction.ADC, AddressMode.ZPG, 3),
        new(0x75, Instruction.ADC, AddressMode.ZPX, 4),
        new(0x6D, Instruction.ADC, AddressMode.ABS, 4),
        new(0x7D, Instruction.ADC, AddressMode.ABX, 4),
        new(0x79, Instruction.ADC, AddressMode.ABY, 4),
        new(0x61, Instruction.ADC, AddressMode.INX, 6),
        new(0x71, Instruction.ADC, AddressMode.INY, 5),

        new(0x29, Instruction.AND, AddressMode.IMM, 2),
        new(0x25, Instruction.AND, AddressMode.ZPG, 3),
        new(0x35, Instruction.AND, AddressMode.ZPX, 4),
        new(0x2D, Instruction.AND, AddressMode.ABS, 4),
        new(0x3D, Instruction.AND, AddressMode.ABX, 4),
        new(0x39, Instruction.AND, AddressMode.ABY, 4),
        new(0x21, Instruction.AND, AddressMode.INX, 6),
        new(0x31, Instruction.AND, AddressMode.INY, 5),

        new(0x0A, Instruction.ASL, AddressMode.IMP, 2),
        new(0x06, Instruction.ASL, AddressMode.ZPG, 5),
        new(0x16, Instruction.ASL, AddressMode.ZPX, 6),
        new(0x0E, Instruction.ASL, AddressMode.ABS, 6),
        new(0x1E, Instruction.ASL, AddressMode.ABX, 7),

        new(0x90, Instruction.BCC, AddressMode.REL, 2),
        new(0xB0, Instruction.BCS, AddressMode.REL, 2),
        new(0xF0, Instruction.BEQ, AddressMode.REL, 2),

        new(0x24, Instruction.BIT, AddressMode.ZPG, 3),
        new(0x2C, Instruction.BIT, AddressMode.ABS, 4),

        new(0x30, Instruction.BMI, AddressMode.REL, 2),
        new(0xD0, Instruction.BNE, AddressMode.REL, 2),
        new(0x10, Instruction.BPL, AddressMode.REL, 2),

        new(0x00, Instruction.BRK, AddressMode.IMP, 7),

        new(0x50, Instruction.BVC, AddressMode.REL, 2),
        new(0x70, Instruction.BVS, AddressMode.REL, 2),

        new(0x18, Instruction.CLC, AddressMode.IMP, 2),
        new(0xD8, Instruction.CLD, AddressMode.IMP, 2),
        new(0x58, Instruction.CLI, AddressMode.IMP, 2),
        new(0xB8, Instruction.CLV, AddressMode.IMP, 2),

        new(0xC9, Instruction.CMP, AddressMode.IMM, 2),
        new(0xC5, Instruction.CMP, AddressMode.ZPG, 3),
        new(0xD5, Instruction.CMP, AddressMode.ZPX, 4),
        new(0xCD, Instruction.CMP, AddressMode.ABS, 4),
        new(0xDD, Instruction.CMP, AddressMode.ABX, 4),
        new(0xD9, Instruction.CMP, AddressMode.ABY, 4),
        new(0xC1, Instruction.CMP, AddressMode.INX, 6),
        new(0xD1, Instruction.CMP, AddressMode.INY, 5),

        new(0xE0, Instruction.CPX, AddressMode.IMM, 2),
        new(0xE4, Instruction.CPX, AddressMode.ZPG, 3),
        new(0xEC, Instruction.CPX, AddressMode.ABS, 4),

        new(0xC0, Instruction.CPY, AddressMode.IMM, 2),
        new(0xC4, Instruction.CPY, AddressMode.ZPG, 3),
        new(0xCC, Instruction.CPY, AddressMode.ABS, 4),

        new(0xC6, Instruction.DEC, AddressMode.ZPG, 5),
        new(0xD6, Instruction.DEC, AddressMode.ZPX, 6),
        new(0xCE, Instruction.DEC, AddressMode.ABS, 6),
        new(0xDE, Instruction.DEC, AddressMode.ABX, 7),

        new(0xCA, Instruction.DEX, AddressMode.IMP, 2),
        new(0x88, Instruction.DEY, AddressMode.IMP, 2),

        new(0x49, Instruction.EOR, AddressMode.IMM, 2),
        new(0x45, Instruction.EOR, AddressMode.ZPG, 3),
        new(0x55, Instruction.EOR, AddressMode.ZPX, 4),
        new(0x4D, Instruction.EOR, AddressMode.ABS, 4),
        new(0x5D, Instruction.EOR, AddressMode.ABX, 4),
        new(0x59, Instruction.EOR, AddressMode.ABY, 4),
        new(0x41, Instruction.EOR, AddressMode.INX, 6),
        new(0x51, Instruction.EOR, AddressMode.INY, 5),

        new(0xE6, Instruction.INC, AddressMode.ZPG, 5),
        new(0xF6, Instruction.INC, AddressMode.ZPX, 6),
        new(0xEE, Instruction.INC, AddressMode.ABS, 6),
        new(0xFE, Instruction.INC, AddressMode.ABX, 7),

        new(0xE8, Instruction.INX, AddressMode.IMP, 2),
        new(0xC8, Instruction.INY, AddressMode.IMP, 2),

        new(0x4C, Instruction.JMP, AddressMode.ABS, 3),
        new(0x6C, Instruction.JMP, AddressMode.IND, 5),
        new(0x20, Instruction.JSR, AddressMode.ABS, 6),

        new(0xA9, Instruction.LDA, AddressMode.IMM, 2),
        new(0XA5, Instruction.LDA, AddressMode.ZPG, 3),
        new(0xB5, Instruction.LDA, AddressMode.ZPX, 4),
        new(0xAD, Instruction.LDA, AddressMode.ABS, 4),
        new(0xBD, Instruction.LDA, AddressMode.ABX, 4),
        new(0xB9, Instruction.LDA, AddressMode.ABY, 4),
        new(0xA1, Instruction.LDA, AddressMode.INX, 6),
        new(0xB1, Instruction.LDA, AddressMode.INY, 5),

        new(0xA2, Instruction.LDX, AddressMode.IMM, 2),
        new(0xA6, Instruction.LDX, AddressMode.ZPG, 3),
        new(0xB6, Instruction.LDX, AddressMode.ZPY, 4),
        new(0xAE, Instruction.LDX, AddressMode.ABS, 4),
        new(0xBE, Instruction.LDX, AddressMode.ABY, 4),

        new(0xA0, Instruction.LDY, AddressMode.IMM, 2),
        new(0xA4, Instruction.LDY, AddressMode.ZPG, 3),
        new(0xB4, Instruction.LDY, AddressMode.ZPX, 4),
        new(0xAC, Instruction.LDY, AddressMode.ABS, 4),
        new(0xBC, Instruction.LDY, AddressMode.ABX, 4),

        new(0x4A, Instruction.LSR, AddressMode.IMP, 2),
        new(0x46, Instruction.LSR, AddressMode.ZPG, 5),
        new(0x56, Instruction.LSR, AddressMode.ZPX, 6),
        new(0x4E, Instruction.LSR, AddressMode.ABS, 6),
        new(0x5E, Instruction.LSR, AddressMode.ABX, 7),

        new(0xEA, Instruction.NOP, AddressMode.IMP, 2),

        new(0x09, Instruction.ORA, AddressMode.IMM, 2),
        new(0x05, Instruction.ORA, AddressMode.ZPG, 3),
        new(0x15, Instruction.ORA, AddressMode.ZPX, 4),
        new(0x0D, Instruction.ORA, AddressMode.ABS, 4),
        new(0x1D, Instruction.ORA, AddressMode.ABX, 4),
        new(0x19, Instruction.ORA, AddressMode.ABY, 4),
        new(0x01, Instruction.ORA, AddressMode.INX, 6),
        new(0x11, Instruction.ORA, AddressMode.INY, 5),

        new(0x48, Instruction.PHA, AddressMode.IMP, 3),
        new(0x08, Instruction.PHP, AddressMode.IMP, 3),
        new(0x68, Instruction.PLA, AddressMode.IMP, 4),
        new(0x28, Instruction.PLP, AddressMode.IMP, 4),

        new(0x2A, Instruction.ROL, AddressMode.IMP, 2),
        new(0x26, Instruction.ROL, AddressMode.ZPG, 5),
        new(0x36, Instruction.ROL, AddressMode.ZPX, 6),
        new(0x2E, Instruction.ROL, AddressMode.ABS, 6),
        new(0x3E, Instruction.ROL, AddressMode.ABX, 7),

        new(0x6A, Instruction.ROR, AddressMode.IMP, 2),
        new(0x66, Instruction.ROR, AddressMode.ZPG, 5),
        new(0x76, Instruction.ROR, AddressMode.ZPX, 6),
        new(0x6E, Instruction.ROR, AddressMode.ABS, 6),
        new(0x7E, Instruction.ROR, AddressMode.ABX, 7),

        new(0x40, Instruction.RTI, AddressMode.IMP, 6),
        new(0x60, Instruction.RTS, AddressMode.IMP, 6),

        new(0xE9, Instruction.SBC, AddressMode.IMM, 2),
        new(0xE5, Instruction.SBC, AddressMode.ZPG, 3),
        new(0xF5, Instruction.SBC, AddressMode.ZPX, 4),
        new(0xED, Instruction.SBC, AddressMode.ABS, 4),
        new(0xFD, Instruction.SBC, AddressMode.ABX, 4),
        new(0xF9, Instruction.SBC, AddressMode.ABY, 4),
        new(0xE1, Instruction.SBC, AddressMode.INX, 6),
        new(0xF1, Instruction.SBC, AddressMode.INY, 5),

        new(0x38, Instruction.SEC, AddressMode.IMP, 2),
        new(0xF8, Instruction.SED, AddressMode.IMP, 2),
        new(0x78, Instruction.SEI, AddressMode.IMP, 2),

        new(0x85, Instruction.STA, AddressMode.ZPG, 3),
        new(0x95, Instruction.STA, AddressMode.ZPX, 4),
        new(0x8D, Instruction.STA, AddressMode.ABS, 4),
        new(0x9D, Instruction.STA, AddressMode.ABX, 5),
        new(0x99, Instruction.STA, AddressMode.ABY, 5),
        new(0x81, Instruction.STA, AddressMode.INX, 6),
        new(0x91, Instruction.STA, AddressMode.INY, 6),

        new(0x86, Instruction.STX, AddressMode.ZPG, 3),
        new(0x96, Instruction.STX, AddressMode.ZPY, 4),
        new(0x8E, Instruction.STX, AddressMode.ABS, 4),

        new(0x84, Instruction.STY, AddressMode.ZPG, 3),
        new(0x94, Instruction.STY, AddressMode.ZPX, 4),
        new(0x8C, Instruction.STY, AddressMode.ABS, 4),

        new(0xAA, Instruction.TAX, AddressMode.IMP, 2),
        new(0xA8, Instruction.TAY, AddressMode.IMP, 2),
        new(0xBA, Instruction.TSX, AddressMode.IMP, 2),
        new(0x8A, Instruction.TXA, AddressMode.IMP, 2),
        new(0x9A, Instruction.TXS, AddressMode.IMP, 2),
        new(0x98, Instruction.TYA, AddressMode.IMP, 2),
    };

    public static IDictionary<byte, Opcode> Map { get; } =
        Table.ToDictionary(x => x.Value);
}
