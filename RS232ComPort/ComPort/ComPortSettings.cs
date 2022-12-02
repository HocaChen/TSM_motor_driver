using static RS232ComPort.ComPort.ComPortEmun;

namespace RS232ComPort.ComPort
{
    public class ComPortSettings
    {
        private string _portNum;
        private PortBauRate _bauRate;
        private PortDataBits _dataBits;
        private PortStopBits _stopBits;
        private PortParity _parity;
        private PortDataFormat _format;

        public ComPortSettings()
        {
            _portNum = @"COM1";
            _bauRate = PortBauRate.BaurRate9600;
            _dataBits = PortDataBits.DataBits8;
            _stopBits = PortStopBits.One;
            _parity = PortParity.None;
            _format = PortDataFormat.Text;
        }

        public string PortNum
        {
            get { return _portNum; }
            set { _portNum = value; }
        }

        public PortBauRate BauRate
        {
            get { return _bauRate; }
            set { _bauRate = value; }
        }

        public PortDataBits DataBits
        {
            get { return _dataBits; }
            set { _dataBits = value; }
        }

        public PortStopBits StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        public PortParity Parity
        {
            get { return _parity; }
            set { _parity = value; }
        }

        public PortDataFormat Format
        {
            get { return _format; }
            set { _format = value; }
        }
    }
}
