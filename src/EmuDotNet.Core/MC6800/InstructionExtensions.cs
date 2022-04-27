namespace EmuDotNet.Core.MC6800;

public static class InstructionExtensions
{
    public static Accumulator GetAccumulator(this Instruction instruction) => instruction switch
    {
        Instruction.PUL_A => Accumulator.A,
        Instruction.PUL_B => Accumulator.B,

        Instruction.PSH_A => Accumulator.A,
        Instruction.PSH_B => Accumulator.B,

        Instruction.NEG_A => Accumulator.A,
        Instruction.NEG_B => Accumulator.B,

        Instruction.COM_A => Accumulator.A,
        Instruction.COM_B => Accumulator.B,

        Instruction.LSR_A => Accumulator.A,
        Instruction.LSR_B => Accumulator.B,

        Instruction.ROR_A => Accumulator.A,
        Instruction.ROR_B => Accumulator.B,

        Instruction.ASR_A => Accumulator.A,
        Instruction.ASR_B => Accumulator.B,

        Instruction.ASL_A => Accumulator.A,
        Instruction.ASL_B => Accumulator.B,

        Instruction.ROL_A => Accumulator.A,
        Instruction.ROL_B => Accumulator.B,

        Instruction.DEC_A => Accumulator.A,
        Instruction.DEC_B => Accumulator.B,

        Instruction.INC_A => Accumulator.A,
        Instruction.INC_B => Accumulator.B,

        Instruction.TST_A => Accumulator.A,
        Instruction.TST_B => Accumulator.B,

        Instruction.CLR_A => Accumulator.A,
        Instruction.CLR_B => Accumulator.B,

        Instruction.SUB_A_IMM => Accumulator.A,
        Instruction.SUB_A_DIR => Accumulator.A,
        Instruction.SUB_A_IDX => Accumulator.A,
        Instruction.SUB_A_EXT => Accumulator.A,
        Instruction.SUB_B_IMM => Accumulator.B,
        Instruction.SUB_B_DIR => Accumulator.B,
        Instruction.SUB_B_IDX => Accumulator.B,
        Instruction.SUB_B_EXT => Accumulator.B,

        Instruction.CMP_A_IMM => Accumulator.A,
        Instruction.CMP_A_DIR => Accumulator.A,
        Instruction.CMP_A_IDX => Accumulator.A,
        Instruction.CMP_A_EXT => Accumulator.A,
        Instruction.CMP_B_IMM => Accumulator.B,
        Instruction.CMP_B_DIR => Accumulator.B,
        Instruction.CMP_B_IDX => Accumulator.B,
        Instruction.CMP_B_EXT => Accumulator.B,

        Instruction.SBC_A_IMM => Accumulator.A,
        Instruction.SBC_A_DIR => Accumulator.A,
        Instruction.SBC_A_IDX => Accumulator.A,
        Instruction.SBC_A_EXT => Accumulator.A,
        Instruction.SBC_B_IMM => Accumulator.B,
        Instruction.SBC_B_DIR => Accumulator.B,
        Instruction.SBC_B_IDX => Accumulator.B,
        Instruction.SBC_B_EXT => Accumulator.B,

        Instruction.AND_A_IMM => Accumulator.A,
        Instruction.AND_A_DIR => Accumulator.A,
        Instruction.AND_A_IDX => Accumulator.A,
        Instruction.AND_A_EXT => Accumulator.A,
        Instruction.AND_B_IMM => Accumulator.B,
        Instruction.AND_B_DIR => Accumulator.B,
        Instruction.AND_B_IDX => Accumulator.B,
        Instruction.AND_B_EXT => Accumulator.B,

        Instruction.BIT_A_IMM => Accumulator.A,
        Instruction.BIT_A_DIR => Accumulator.A,
        Instruction.BIT_A_IDX => Accumulator.A,
        Instruction.BIT_A_EXT => Accumulator.A,
        Instruction.BIT_B_IMM => Accumulator.B,
        Instruction.BIT_B_DIR => Accumulator.B,
        Instruction.BIT_B_IDX => Accumulator.B,
        Instruction.BIT_B_EXT => Accumulator.B,

        Instruction.LDA_A_IMM => Accumulator.A,
        Instruction.LDA_A_DIR => Accumulator.A,
        Instruction.LDA_A_IDX => Accumulator.A,
        Instruction.LDA_A_EXT => Accumulator.A,
        Instruction.LDA_B_IMM => Accumulator.B,
        Instruction.LDA_B_DIR => Accumulator.B,
        Instruction.LDA_B_IDX => Accumulator.B,
        Instruction.LDA_B_EXT => Accumulator.B,

        Instruction.STA_A_DIR => Accumulator.A,
        Instruction.STA_A_IDX => Accumulator.A,
        Instruction.STA_A_EXT => Accumulator.A,
        Instruction.STA_B_DIR => Accumulator.B,
        Instruction.STA_B_IDX => Accumulator.B,
        Instruction.STA_B_EXT => Accumulator.B,

        Instruction.EOR_A_IMM => Accumulator.A,
        Instruction.EOR_A_DIR => Accumulator.A,
        Instruction.EOR_A_IDX => Accumulator.A,
        Instruction.EOR_A_EXT => Accumulator.A,
        Instruction.EOR_B_IMM => Accumulator.B,
        Instruction.EOR_B_DIR => Accumulator.B,
        Instruction.EOR_B_IDX => Accumulator.B,
        Instruction.EOR_B_EXT => Accumulator.B,

        Instruction.ADC_A_IMM => Accumulator.A,
        Instruction.ADC_A_DIR => Accumulator.A,
        Instruction.ADC_A_IDX => Accumulator.A,
        Instruction.ADC_A_EXT => Accumulator.A,
        Instruction.ADC_B_IMM => Accumulator.B,
        Instruction.ADC_B_DIR => Accumulator.B,
        Instruction.ADC_B_IDX => Accumulator.B,
        Instruction.ADC_B_EXT => Accumulator.B,

        Instruction.ORA_A_IMM => Accumulator.A,
        Instruction.ORA_A_DIR => Accumulator.A,
        Instruction.ORA_A_IDX => Accumulator.A,
        Instruction.ORA_A_EXT => Accumulator.A,
        Instruction.ORA_B_IMM => Accumulator.B,
        Instruction.ORA_B_DIR => Accumulator.B,
        Instruction.ORA_B_IDX => Accumulator.B,
        Instruction.ORA_B_EXT => Accumulator.B,

        Instruction.ADD_A_IMM => Accumulator.A,
        Instruction.ADD_A_DIR => Accumulator.A,
        Instruction.ADD_A_IDX => Accumulator.A,
        Instruction.ADD_A_EXT => Accumulator.A,
        Instruction.ADD_B_IMM => Accumulator.B,
        Instruction.ADD_B_DIR => Accumulator.B,
        Instruction.ADD_B_IDX => Accumulator.B,
        Instruction.ADD_B_EXT => Accumulator.B,

        Instruction.CPX_A_IMM => Accumulator.A,
        Instruction.CPX_A_DIR => Accumulator.A,
        Instruction.CPX_A_IDX => Accumulator.A,
        Instruction.CPX_A_EXT => Accumulator.A,
        _ => throw new ArgumentException($"Instruction {instruction} does not specify an accumulator", nameof(instruction))
    };

    public static AddressingMode GetMode(this Instruction instruction)
    {
        var nibble = (byte) instruction >> 4;

        return nibble switch
        {
            0x2 => AddressingMode.REL,
            0x6 => AddressingMode.IDX,
            0x7 => AddressingMode.EXT,
            0x8 when instruction == Instruction.BSR => AddressingMode.REL,
            0x8 => AddressingMode.IMM,
            0x9 => AddressingMode.DIR,
            0xA => AddressingMode.IDX,
            0xB => AddressingMode.EXT,
            0xC => AddressingMode.IMM,
            0xD => AddressingMode.DIR,
            0xE => AddressingMode.IDX,
            0xF => AddressingMode.EXT,
            _ => throw new ArgumentException($"Instruction {instruction} does not specify an addressing mode", nameof(instruction))
        };
    }
}
