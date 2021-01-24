namespace EmuDotNet.Core
{
    public class MC6800
    {
        private readonly MC6800Registers _registers = new();

        #region Pins

        /// <summary>
        ///  HALT Pin
        /// </summary>
        public bool Halt { get; set; }

        /// <summary>
        /// IRQ Input Pin
        /// </summary>
        /// <remarks>
        /// Requests an interrupt sequence generation within the processor
        /// </remarks>
        public bool InterruptRequest { get; set; }

        /// <summary>
        /// VMA Pin
        /// </summary>
        /// <remarks>
        /// Address loaded on address bus is valid
        /// </remarks>
        public bool ValidMemoryAddress { get; private set; }

        /// <summary>
        /// NMI Pin
        /// </summary>
        /// <remarks>
        /// Hardware interrupt
        /// </remarks>
        public bool NonMaskableInterrupt { get; set; }

        /// <summary>
        /// BA Pin
        /// </summary>
        /// <remarks>
        /// Processor has stopped and address bus is available
        /// </remarks>
        public bool BusAvailable { get; private set; }

        /// <summary>
        /// 16-bit address bus
        /// </summary>
        public ushort AddressBus { get; set; }

        /// <summary>
        /// 8-bit data bus
        /// </summary>
        public byte DataBus { get; set; }

        /// <summary>
        /// R/W Pin
        /// </summary>
        /// <remarks>
        /// On TRUE, reading from memory. on FALSE, writing to memory
        /// </remarks>
        public bool ReadWrite { get; set; }

        /// <summary>
        /// DBE Pin
        /// </summary>
        public bool DataBusEnable { get; set; }

        /// <summary>
        /// TSC Pin
        /// </summary>
        public bool ThreeStateControl { get; set; }

        /// <summary>
        /// RESET Pin
        /// </summary>
        public bool Reset { get; set; }

        #endregion
    }
}
