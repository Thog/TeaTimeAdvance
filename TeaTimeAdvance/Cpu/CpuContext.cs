using TeaTimeAdvance.Bus;
using TeaTimeAdvance.Cpu.Instruction;
using TeaTimeAdvance.Cpu.State;
using TeaTimeAdvance.Scheduler;

namespace TeaTimeAdvance.Cpu
{
    public class CpuContext
    {
        public CpuState State { get; }
        public CpuPipeline Pipeline { get; }
        public SchedulerContext Scheduler { get; }
        public BusContext BusContext { get; }

        public BusAccessType BusAccessType { get => Pipeline.BusAccessType; set => Pipeline.BusAccessType = value; }

        public CpuContext(SchedulerContext scheduler, BusContext busContext)
        {
            Scheduler = scheduler;
            BusContext = busContext;

            State = new CpuState();
            Pipeline = new CpuPipeline();
        }

        public void Reset()
        {
            Pipeline.Reset();
            State.Reset();
        }

        public void SetRegister(CpuRegister register, uint value)
        {
            ref uint reg = ref State.Register(register);

            reg = value;
        }

        public uint GetRegister(CpuRegister register)
        {
            return State.Register(register);
        }

        public void UpdateProgramCounter32()
        {
            State.Register(CpuRegister.PC) += InstructionDecoderHelper.ArmInstructionSize;
        }

        public void UpdateProgramCounter16()
        {
            State.Register(CpuRegister.PC) += InstructionDecoderHelper.ThumbInstructionSize;
        }

        public void ReloadPipeline()
        {
            if (State.StatusRegister.HasFlag(CurrentProgramStatusRegister.Thumb))
            {
                Pipeline.ReloadForThumb(this);
            }
            else
            {
                Pipeline.ReloadForArm(this);
            }
        }

        public void SetStatusFlag(CurrentProgramStatusRegister field, bool value)
        {
            State.SetStatusFlag(field, value);
        }

        public void SetCpuMode(CpuMode mode)
        {
            State.SetCpuMode(mode);
        }

        public void Update()
        {
            Pipeline.Update(this);
        }

        public void Idle()
        {
            Scheduler.UpdateCycles(1);
        }
    }
}
