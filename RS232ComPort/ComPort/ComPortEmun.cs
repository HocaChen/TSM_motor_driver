using System.IO.Ports;

namespace RS232ComPort.ComPort
{
    public class ComPortEmun
    {
        public enum PortBauRate
        {
            BaurRate300,
            BaurRate1200,
            BaurRate2400,
            BaurRate4800,
            BaurRate9600,
            BaurRate14400,
            BaurRate19200,
            BaurRate28800,
            BaurRate38400,
            BaurRate57600,
            BaurRate115200,
            BaurRate230400,
        }

        public enum PortDataBits
        {
            DataBits5,
            DataBits6,
            DataBits7,
            DataBits8,
            DataBits9,
        }

        public enum PortStopBits
        {
            None = StopBits.None,
            One = StopBits.One,
            Two = StopBits.Two,
            OnePointFive = StopBits.OnePointFive,
        }

        public enum PortParity
        {
            None = Parity.None,
            Odd = Parity.Odd,
            Even = Parity.Even,
            Mark = Parity.Mark,
            Space = Parity.Space,
        }

        public enum PortDataFormat
        {
            Text,
            Hex,
        }

        public enum ComPortChecker
        {
            None,
            CheckSum,
        }

    }
}
