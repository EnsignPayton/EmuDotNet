﻿namespace EmuDotNet.Core.MC6800;

public enum Instruction
{
    NOP = 0x01,
    TAP = 0x06,
    TPA = 0x07,
    INX = 0x08,
    DEX = 0x09,
    CLV = 0x0A,
    SEV = 0x0B,
    CLC = 0x0C,
    SEC = 0x0D,
    CLI = 0x0E,
    SEI = 0x0F,
    SBA = 0x10,
    CBA = 0x11,
    TAB = 0x16,
    TBA = 0x17,
    DAA = 0x19,
    ABA = 0x1B,
    BRA = 0x20,
    BHI = 0x22,
    BLS = 0x23,
    BCC = 0x24,
    BCS = 0x25,
    BNE = 0x26,
    BEQ = 0x27,
    BVC = 0x28,
    BVS = 0x29,
    BPL = 0x2A,
    BMI = 0x2B,
    BGE = 0x2C,
    BLT = 0x2D,
    BGT = 0x2E,
    BLE = 0x2F,
    TSX = 0x30,
    INS = 0x31,
    PUL_A = 0x32,
    PUL_B = 0x33,
    DES = 0x34,
    TXS = 0x35,
    PSH_A = 0x36,
    PSH_B = 0x37,
    RTS = 0x39,
    RTI = 0x3B,
    WAI = 0x3E,
    SWI = 0x3F,
    NEG_A = 0x40,
    COM_A = 0x43,
    LSR_A = 0x44,
    ROR_A = 0x46,
    ASR_A = 0x47,
    ASL_A = 0x48,
    ROL_A = 0x49,
    DEC_A = 0x4A,
    INC_A = 0x4C,
    TST_A = 0x4D,
    CLR_A = 0x4F,
    NEG_B = 0x50,
    COM_B = 0x53,
    LSR_B = 0x54,
    ROR_B = 0x56,
    ASR_B = 0x57,
    ASL_B = 0x58,
    ROL_B = 0x59,
    DEC_B = 0x5A,
    INC_B = 0x5C,
    TST_B = 0x5D,
    CLR_B = 0x5F,
    NEG_IDX = 0x60,
    COM_IDX = 0x63,
    LSR_IDX = 0x64,
    ROR_IDX = 0x66,
    ASR_IDX = 0x67,
    ASL_IDX = 0x68,
    ROL_IDX = 0x69,
    DEC_IDX = 0x6A,
    INC_IDX = 0x6C,
    TST_IDX = 0x6D,
    JMP_IDX = 0x6E,
    CLR_IDX = 0x6F,
    NEG_EXT = 0x70,
    COM_EXT = 0x73,
    LSR_EXT = 0x74,
    ROR_EXT = 0x76,
    ASR_EXT = 0x77,
    ASL_EXT = 0x78,
    ROL_EXT = 0x79,
    DEC_EXT = 0x7A,
    INC_EXT = 0x7C,
    TST_EXT = 0x7D,
    JMP_EXT = 0x7E,
    CLR_EXT = 0x7F,
    SUB_A_IMM = 0x80,
    CMP_A_IMM = 0x81,
    SBC_A_IMM = 0x82,
    AND_A_IMM = 0x84,
    BIT_A_IMM = 0x85,
    LDA_A_IMM = 0x86,
    EOR_A_IMM = 0x88,
    ADC_A_IMM = 0x89,
    ORA_A_IMM = 0x8A,
    ADD_A_IMM = 0x8B,
    CPX_A_IMM = 0x8C,
    BSR = 0x8D,
    LDS_IMM = 0x8E,
    SUB_A_DIR = 0x90,
    CMP_A_DIR = 0x91,
    SBC_A_DIR = 0x92,
    AND_A_DIR = 0x94,
    BIT_A_DIR = 0x95,
    LDA_A_DIR = 0x96,
    STA_A_DIR = 0x97,
    EOR_A_DIR = 0x98,
    ADC_A_DIR = 0x99,
    ORA_A_DIR = 0x9A,
    ADD_A_DIR = 0x9B,
    CPX_A_DIR = 0x9C,
    LDS_DIR = 0x9E,
    STS_DIR = 0x9F,
    SUB_A_IDX = 0xA0,
    CMP_A_IDX = 0xA1,
    SBC_A_IDX = 0xA2,
    AND_A_IDX = 0xA4,
    BIT_A_IDX = 0xA5,
    LDA_A_IDX = 0xA6,
    STA_A_IDX = 0xA7,
    EOR_A_IDX = 0xA8,
    ADC_A_IDX = 0xA9,
    ORA_A_IDX = 0xAA,
    ADD_A_IDX = 0xAB,
    CPX_A_IDX = 0xAC,
    JSR_IDX = 0xAD,
    LDS_IDX = 0xAE,
    STS_IDX = 0xAF,
    SUB_A_EXT = 0xB0,
    CMP_A_EXT = 0xB1,
    SBC_A_EXT = 0xB2,
    AND_A_EXT = 0xB4,
    BIT_A_EXT = 0xB5,
    LDA_A_EXT = 0xB6,
    STA_A_EXT = 0xB7,
    EOR_A_EXT = 0xB8,
    ADC_A_EXT = 0xB9,
    ORA_A_EXT = 0xBA,
    ADD_A_EXT = 0xBB,
    CPX_A_EXT = 0xBC,
    JSR_EXT = 0xBD,
    LDS_EXT = 0xBE,
    STS_EXT = 0xBF,
    SUB_B_IMM = 0xC0,
    CMP_B_IMM = 0xC1,
    SBC_B_IMM = 0xC2,
    AND_B_IMM = 0xC4,
    BIT_B_IMM = 0xC5,
    LDA_B_IMM = 0xC6,
    EOR_B_IMM = 0xC8,
    ADC_B_IMM = 0xC9,
    ORA_B_IMM = 0xCA,
    ADD_B_IMM = 0xCB,
    LDX_IMM = 0xCE,
    SUB_B_DIR = 0xD0,
    CMP_B_DIR = 0xD1,
    SBC_B_DIR = 0xD2,
    AND_B_DIR = 0xD4,
    BIT_B_DIR = 0xD5,
    LDA_B_DIR = 0xD6,
    STA_B_DIR = 0xD7,
    EOR_B_DIR = 0xD8,
    ADC_B_DIR = 0xD9,
    ORA_B_DIR = 0xDA,
    ADD_B_DIR = 0xDB,
    LDX_DIR = 0xDE,
    STX_DIR = 0xDF,
    SUB_B_IDX = 0xE0,
    CMP_B_IDX = 0xE1,
    SBC_B_IDX = 0xE2,
    AND_B_IDX = 0xE4,
    BIT_B_IDX = 0xE5,
    LDA_B_IDX = 0xE6,
    STA_B_IDX = 0xE7,
    EOR_B_IDX = 0xE8,
    ADC_B_IDX = 0xE9,
    ORA_B_IDX = 0xEA,
    ADD_B_IDX = 0xEB,
    LDX_IDX = 0xEE,
    STX_IDX = 0xEF,
    SUB_B_EXT = 0xF0,
    CMP_B_EXT = 0xF1,
    SBC_B_EXT = 0xF2,
    AND_B_EXT = 0xF4,
    BIT_B_EXT = 0xF5,
    LDA_B_EXT = 0xF6,
    STA_B_EXT = 0xF7,
    EOR_B_EXT = 0xF8,
    ADC_B_EXT = 0xF9,
    ORA_B_EXT = 0xFA,
    ADD_B_EXT = 0xFB,
    LDX_EXT = 0xFE,
    STX_EXT = 0xFF,
}
