using RS232ComPort.ComPort;
using RS232ComPort.RemoteMotion;
using System;
using System.Globalization;
using static RS232ComPort.ComPort.ComPortEmun;

namespace TSM_Motor
{
    class TSM_Motor
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TSM Motor Driver");
            var driver = new Driver();

            //---Create and open the Com Port--//
            driver.Init();

            //--- Motor ABS Move--//
            driver.Move();

            //--- Motor Jog Move --//
            driver.Jog();

            //--- Motor Step Move--//
            driver.Step();

            //--- Motor Stop---//
            driver.Stop();

            //--- Motor Status---//
            driver.GetStatus();
        }
    }

    class Driver
    {
        private ComPortBase _comPort;

        public void Init()
        {
            Console.WriteLine("Init");
            _comPort = new ComPortBase
            {
                Settings =
                {
                    BauRate = PortBauRate.BaurRate9600,
                    Parity = PortParity.None,
                    DataBits = PortDataBits.DataBits8,
                    StopBits = PortStopBits.One,
                    Format = PortDataFormat.Hex
                }
            };
            _comPort.StartBytes = null;
            _comPort.StopBytes = new byte[1];
            _comPort.StopBytes[0] = RemoteMotionConstants.CR;
            _comPort.BytesChecker = ComPortChecker.None;
            _comPort.Settings.PortNum = "COM1";

            _comPort.Open(_comPort.Settings);


        }

        internal void GetStatus()
        {
            string cmd;

            /*
            AxisName can be define at MotorDriver
            e.g.: 
            X axis = 1
            Y axis = 2
            Z axis = 3
            */
            var axisName = "1";

            //-- Reference Mauanl (Host-Command-Reference_920-0002N.PDF) Page 240
            uint motorStatus;

            cmd = $"{axisName + RemoteMotionConstants.RquestStatus}";
            var receiveData = _comPort.SendAndReceive(cmd);

            if (receiveData.Contains(RemoteMotionConstants.RquestStatus))
            {
                var eIndex = receiveData.IndexOf("\r", StringComparison.Ordinal);
                var sIndex = receiveData.IndexOf("=", StringComparison.Ordinal) + 1;
                var length = eIndex - sIndex;

                var substr = receiveData.Substring(sIndex, length);
                try
                {
                    motorStatus = uint.Parse(substr, NumberStyles.AllowHexSpecifier);
                }
                catch (Exception)
                {
                    Console.WriteLine("Format incorrect");
                }
            }

            /*
             * Hex Value Status Code bit definition
            0001 Motor Enabled (Motor Disabled if this bit = 0)
            0002 Sampling (for Quick Tuner)
            0004 Drive Fault (check Alarm Code)
            0008 In Position (motor is in position)
            0010 Moving (motor is moving)
            0020 Jogging (currently in jog mode)
            0040 Stopping (in the process of stopping from a stop command)
            0080 Waiting (for an input; executing a WI command)
            0100 Saving (parameter data is being saved)
            0200 Alarm present (check Alarm Code)
            0400 Homing (executing an SH command)
            0800 Waiting (for time; executing a WD or WT command)
            1000 Wizard running (Timing Wizard is running)
            2000 Checking encoder (Timing Wizard is running)
            4000 Q Program is running
            8000 Initializing (happens at power up) ; Servo Ready (for SV200 drives only
             * */
        }

        internal void Jog()
        {
            string cmd;
            /*
            AxisName can be define at MotorDriver
            e.g.: 
            X axis = 1
            Y axis = 2
            Z axis = 3
            */
            var axisName = "1";

            //-- Define direction--Pos:1, Neg:-1//

            cmd = $"{axisName + RemoteMotionConstants.SetDistance + "1"}";
          //cmd = $"{axisName + RemoteMotionConstants.SetDistance + "-1"}";

            string receiveData = _comPort.SendAndReceive(cmd);

            ReceiveChecking(receiveData);

            cmd = $"{axisName + RemoteMotionConstants.ExecuteJogging}";

            receiveData = _comPort.SendAndReceive(cmd);
            ReceiveChecking(receiveData);
        }

        internal void Move()
        {

            string cmd;
            //---Define motor count for movement, This area needs to translete the "phycisal distance to Motor Count"---//
            //--- e.g. 1 Motor Count = 0.1 um, if I want to move 100 um, count needs to be 1000 ---//
            var scopeDeviceCount = 1000;

            /*
            AxisName can be define at MotorDriver
            e.g.: 
            X axis = 1
            Y axis = 2
            Z axis = 3
            */
            var axisName = "1";
            cmd = $"{axisName + RemoteMotionConstants.SetDistance + Convert.ToString(scopeDeviceCount)}";
            var receiveData = _comPort.SendAndReceive(cmd);

            ReceiveChecking(receiveData);

            cmd = $"{axisName + RemoteMotionConstants.ExecuteAbsoluteMove}";
            receiveData = _comPort.SendAndReceive(cmd);

            ReceiveChecking(receiveData);
        }

        internal void Step()
        {
            string cmd;
            //---Define motor count for movement, This area needs to translete the "phycisal distance to Motor Count"---//
            //--- e.g. 1 Motor Count = 0.1 um, if I want to move 100 um, count needs to be 1000 ---//
            var scopeDeviceCount = 1000;

            /*
            AxisName can be define at MotorDriver
            e.g.: 
            X axis = 1
            Y axis = 2
            Z axis = 3
            */
            var axisName = "1";
            cmd = $"{axisName + RemoteMotionConstants.SetDistance + Convert.ToString(scopeDeviceCount)}";
            var receiveData = _comPort.SendAndReceive(cmd);

            ReceiveChecking(receiveData);

            cmd = $"{axisName + RemoteMotionConstants.ExecuteRelativeMove}";
            receiveData = _comPort.SendAndReceive(cmd);

            ReceiveChecking(receiveData);
        }

        internal void Stop()
        {
            /*
            AxisName can be define at MotorDriver
            e.g.: 
            X axis = 1
            Y axis = 2
            Z axis = 3
            */
            var axisName = "1";

            // Reference Mauanl (Host-Command-Reference_920-0002N.PDF) Page 252
            var cmd = $"{axisName + RemoteMotionConstants.StopMotor}";

            var receiveData = _comPort.SendAndReceive(cmd);
            ReceiveChecking(receiveData);
        }

        private bool ReceiveChecking(string receiveData)
        {
            if (receiveData.Contains("%") || receiveData.Contains("*"))
            {
                return true;
            }
            else
                return false;
        }
    }
}
