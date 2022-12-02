using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using static RS232ComPort.ComPort.ComPortEmun;

namespace RS232ComPort.ComPort
{
    public class ComPortBase
    {
        private ComPortEventArgs _args;
        private SerialPort _port;

        private List<byte> _listBuff;
        private List<byte> _listData;
        private List<byte> _listCmd;

        public delegate void ComPortDelegate(object sender, ComPortEventArgs e);
        public event ComPortDelegate ComPortCallback;

        public ComPortBase()
        {
            StartBytes = null;
            StopBytes = null;

            _listBuff = new List<byte>();
            _listData = new List<byte>();
            _listCmd = new List<byte>();

            _port = new SerialPort();
            _args = new ComPortEventArgs();
            Settings = new ComPortSettings();

            BytesChecker = ComPortChecker.None;

            _port.PinChanged -= _port_PinChanged;
            _port.PinChanged += _port_PinChanged;
            _port.DataReceived -= _port_DataReceived;
            _port.DataReceived += _port_DataReceived;
            _port.ErrorReceived -= _port_ErrorReceived;
            _port.ErrorReceived += _port_ErrorReceived;
        }

        private void _port_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Args = new ComPortEventArgs() { ErrorReceived = e };
        }

        private void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Args = new ComPortEventArgs() { DataReceived = e };
        }

        private void _port_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            Args = new ComPortEventArgs() { PinChanged = e };
        }



        #region Property

        public byte[] StartBytes { get; set; }
        public byte[] StopBytes { get; set; }

        private int ReadBytes { get; set; }
        private byte[] ArrayBuff { get; set; }


        public ComPortSettings Settings { get; set; }
        public ComPortChecker BytesChecker { get; set; }

        public ComPortEventArgs Args
        {
            get { return _args; }
            private set
            {
                _args = value;

                if (ComPortCallback == null) return;

                if (value.DataReceived != null)
                {
                    ReadBytes = _port.BytesToRead;
                    ArrayBuff = new byte[ReadBytes];
                    _port.Read(ArrayBuff, 0, ReadBytes);
                    // Paser by stop byte(s)
                    if (StopBytes != null && StopBytes.Length > 0)
                    {
                        foreach (byte b in ArrayBuff)
                        {
                            _listBuff.Add(b);
                        }
                        if (!_listBuff.Contains(StopBytes[StopBytes.Length - 1])) return;   // Check the end of byte is END byte or not

                        _listData.Clear();
                        foreach (byte b in _listBuff)
                        {
                            if (b == StopBytes[0])   // delect the StopBytes[0]
                                break;
                            _listData.Add(b);
                        }
                        _args.ReceivedBytes = new byte[_listData.Count];
                        _args.ReceivedBytes = _listData.ToArray();  // Receive the all data to _args.ReceivedBytes
                        ComPortCallback(this, _args);
                        _listBuff.Clear();
                    }
                }
                else if (value.ErrorReceived != null)
                {
                    // log the Error Received
                }
                else if (value.PinChanged != null)
                {
                    // log the Pin change Received
                }
                else
                    ComPortCallback(this, _args);
            }
        }

        #endregion

        #region Public function

        public bool Open(ComPortSettings settings)
        {
            bool ret;

            try
            {
                Settings = settings;
                _port.PortName = Settings.PortNum;
                _port.BaudRate = Convert.ToInt32(Settings.BauRate.ToString().Substring(8, Settings.BauRate.ToString().Length - 8));
                _port.DataBits = Convert.ToInt32(Settings.DataBits.ToString().Substring(8, Settings.DataBits.ToString().Length - 8));
                _port.StopBits = (StopBits)Settings.StopBits;
                _port.Parity = (Parity)Settings.Parity;
                _port.Open();
                ret = true;
            }
            catch (FormatException format)
            {
                Args = new ComPortEventArgs { ErrorMsg = format.Message };
                ret = false;
            }
            catch (IOException io)
            {
                Args = new ComPortEventArgs { ErrorMsg = io.Message };
                ret = false;
            }
            catch (Exception e)
            {
                Args = new ComPortEventArgs { ErrorMsg = e.Message };
                ret = false;
            }

            return ret;
        }

        public bool Close()
        {
            var ret = false;

            try
            {
                _port.Close();
                ret = true;
            }
            catch (FormatException format)
            {
                Args = new ComPortEventArgs { ErrorMsg = format.Message };
                ret = false;
            }
            catch (IOException io)
            {
                Args = new ComPortEventArgs { ErrorMsg = io.Message };
                ret = false;
            }
            catch (Exception e)
            {
                Args = new ComPortEventArgs { ErrorMsg = e.Message };
                ret = false;
            }

            return ret;
        }

        public bool Write(byte[] msgBytes)
        {
            bool ret;
            if (!_port.IsOpen)
            {
                Args = new ComPortEventArgs { ErrorMsg = "COM Port is not open" };
                return false;
            }
            try
            {
                _listCmd.Clear();
                // Standard format:
                // "Start byte" "Data byte(s)" "Stop byte(s)" "Bytes checker"
                // Start byte(s)
                if (StartBytes != null && StartBytes.Length > 0)
                {
                    foreach (byte b in StartBytes)
                    {
                        _listCmd.Add(b);
                    }
                }
                // Data byte(s)
                if (msgBytes != null && msgBytes.Length > 0)
                {
                    foreach (byte b in msgBytes)
                    {
                        _listCmd.Add(b);
                    }
                }
                // Stop byte(s)
                if (StopBytes != null && StopBytes.Length > 0)
                {
                    foreach (byte b in StopBytes)
                    {
                        _listCmd.Add(b);
                    }
                }
                // Bytes checker
                if (BytesChecker == ComPortChecker.CheckSum)
                {
                    _listCmd.Add(SubFunction.GetCheckSumByte(_listCmd.ToArray()));
                }
                _port.Write(_listCmd.ToArray(), 0, _listCmd.Count);
                ret = true;
            }
            catch (FormatException format)
            {
                Args = new ComPortEventArgs { ErrorMsg = format.Message };
                ret = false;
            }
            catch (IOException io)
            {
                Args = new ComPortEventArgs { ErrorMsg = io.Message };
                ret = false;
            }
            catch (Exception e)
            {
                Args = new ComPortEventArgs { ErrorMsg = e.Message };
                ret = false;
            }

            return ret;
        }

        public bool IsOpen
        {
            get { return _port.IsOpen; }
        }

        public object Lock { get; private set; }

        // Read RS232 is set as Event trigger

        public string SendAndReceive(string msg)
        {

            int retry = 3;
            var resp = string.Empty;
            var cnt = 0;
            for (int t = 0; t < retry; t++)
            {
                try
                {
                    var cmdFail = false;
                    resp = Query(msg, ref cmdFail);
                    if (cmdFail)
                    {
                        Thread.Sleep(50);
                    }
                    else
                        break;
                }
                catch (Exception exc)
                {
                    Thread.Sleep(500);
                    throw new COMException($"RS232 exception:{exc.Message}");
                }
                cnt++;
            }

            if (cnt >= 3 && resp == string.Empty)
            {
                throw new COMException($"RS232 communication fail, command does not have response");
            }

            return resp;
        }

        private string Query(string msg, ref bool _isFail)
        {
            lock (Lock)
            {
                _isFail = false;

                var strResp = string.Empty;
                // Console.WriteLine($"RS232 - Send:{msg}");
                Write(Encoding.ASCII.GetBytes(msg));

                var stopByte = Convert.ToChar(StopBytes[0]);
                if (StopBytes != null && StopBytes.Length > 0)
                    stopByte = Convert.ToChar(StopBytes[StopBytes.Length - 1]);
                Thread.Sleep(100);

                char lastChar;
                var hasTimeout = false;
                do
                {
                    int lengthReceive;
                    var cntTimeout = 0;

                    lengthReceive = _port.BytesToRead;
                    while (lengthReceive == 0 && !hasTimeout)
                    {
                        lengthReceive = _port.BytesToRead;
                        Thread.Sleep(100);
                        cntTimeout++;
                        if (cntTimeout > 10)
                        {
                            // Console.WriteLine($"RS232 - timeout:{msg}");
                            hasTimeout = true;
                        }
                    }

                    if (hasTimeout) break;

                    //Console.WriteLine($"RS232 - Finish_Receive:{msg}");

                    var resp = new char[lengthReceive];
                    _port.Read(resp, 0, lengthReceive);
                    lastChar = resp[lengthReceive - 1];
                    strResp += new string(resp);
                } while (lastChar != stopByte);


                // Console.WriteLine($"RS232 - Finish:{strResp}");
                if (hasTimeout)
                {
                    _isFail = true;
                }

                _port.DiscardOutBuffer();
                _port.DiscardInBuffer();

                return strResp;
            }
        }

        #endregion

    }
}
