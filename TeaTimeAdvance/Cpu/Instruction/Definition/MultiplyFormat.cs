﻿using System.Runtime.InteropServices;
using TeaTimeAdvance.Cpu.State;

namespace TeaTimeAdvance.Cpu.Instruction.Definition
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 4)]
    public struct MulitplyFormat : IInstructionFormat
    {
        public uint Opcode;

        uint IInstructionFormat.Opcode => Opcode;

        public CpuRegister Rm => ((IInstructionFormat)this).GetRegisterByIndex(0);
        public CpuRegister Rs => ((IInstructionFormat)this).GetRegisterByIndex(2);
        public CpuRegister Rn => ((IInstructionFormat)this).GetRegisterByIndex(3);
        public CpuRegister Rd => ((IInstructionFormat)this).GetRegisterByIndex(4);
        public bool SetCondition => (Opcode & (1 << 20)) != 0;
        public bool IsAccumulator => (Opcode & (1 << 21)) != 0;
    }
}