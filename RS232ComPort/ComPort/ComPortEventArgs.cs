using System;
using System.IO.Ports;

namespace RS232ComPort.ComPort
{
    public class ComPortEventArgs: EventArgs
    {
        public byte[] ReceivedBytes = null;
        public string ErrorMsg = "";

        public SerialErrorReceivedEventArgs ErrorReceived = null;
        public SerialDataReceivedEventArgs DataReceived = null;
        public SerialPinChangedEventArgs PinChanged = null;
    }
}
