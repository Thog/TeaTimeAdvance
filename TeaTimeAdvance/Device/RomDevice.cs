using System;

namespace TeaTimeAdvance.Device
{
    public class RomDevice : MemoryBackedDevice
    {
        //public override uint MappedSize => 0x6000000;

        // TODO: Handles weird out of range behaviours.
        public RomDevice(ReadOnlySpan<byte> data) : base(data) { }

        public override void Write8(uint address, byte value)
        {
            // No operations
        }

        public override void Write16(uint address, ushort value)
        {
            // No operations
        }

        public override void Write32(uint address, uint value)
        {
            // No operations
        }
    }
}
