namespace RS232ComPort.RemoteMotion
{
    public class RemoteMotionConstants
    {
        #region Constant for communicating

        internal const byte NL = 0x0A;   // Not acknowledged
        internal const byte CR = 0x0D;   // Not acknowledged

        #endregion

        #region Constant for command set

        //--------Jog Move---------------//
        internal const string SetJogAcceleration    = @"JA";   // Range: 0.167 - 5461.167 (rps)
        internal const string SetJogDeceleration    = @"JL";   // Range: 0.167 - 5461.167 (rps)
        internal const string SetJogSpeed           = @"JS";   // Range: STM: 0.0042 - 80.0000 (rps)
        internal const string ExecuteJogging        = @"CJ";
        //--------Step Move--------------//
        internal const string SetAcceleration       = @"AC";        // Range: 0.167 - 5461.167 (rps)
        internal const string SetDeceleration       = @"DE";        // Range: 0.167 - 5461.167 (rps)
        internal const string SetVelocity           = @"VE";        //Range : 0.0042 - 80.0000 (rev/sec)
        internal const string SetDistance           = @"DI";        // steps of motor
        internal const string ExecuteAbsoluteMove   = @"FP";        
        internal const string ExecuteRelativeMove   = @"FL"; 
       
        //------ Test IO--- -------------//
        internal const string TestIO1L          = @"TI1L";
        internal const string TestIO1H          = @"TI1H";
        internal const string TestIO2L          = @"TI2L";
        internal const string TestIO2H          = @"TI2H";
        internal const string TestIO3L          = @"TI3L";
        internal const string TestIO3H          = @"TI3H";
        internal const string TestIO4L          = @"TI4L";
        internal const string TestIO4H          = @"TI4H";

        //------ Stop Motor -------------//
        internal const string StopMotor         = @"STD";

        //------Search Sensor from IO----//
        internal const string SearchSensor1Low  = @"FS1L";
        internal const string SearchSensor1Hi   = @"FS1H";
        internal const string SearchSensor2Low  = @"FS2L";
        internal const string SearchSensor2Hi   = @"FS2H";
        internal const string SearchSensor3Low  = @"FS3L";
        internal const string SearchSensor3Hi   = @"FS3H";
        internal const string SearchSensor4Low  = @"FS4L";
        internal const string SearchSensor4Hi   = @"FS4H";

        //------Search Sensor from IO----//
        internal const string SeekHomeIO1Low    = @"SH1L";
        internal const string SeekHomeIO1Hi     = @"SH1H";
        internal const string SeekHomeIO2Low    = @"SH2L";
        internal const string SeekHomeIO2Hi     = @"SH2H";
        internal const string SeekHomeIO3Low    = @"SH3L";
        internal const string SeekHomeIO3Hi     = @"SH3H";
        internal const string SeekHomeIO4Low    = @"SH4L";
        internal const string SeekHomeIO4Hi     = @"SH4H";

        //------Limit setting--------------//
        internal const string SetLimitClose     = @"DL1";
        internal const string SetLimitOpen      = @"DL2";
        internal const string SetLimitIO        = @"DL3";


        //------Q Programming------//
        internal const string ExecuteQSeqment1  = @"QX1";
        internal const string ExecuteQSeqment2  = @"QX2";
        internal const string ExecuteQSeqment3  = @"QX3";
        internal const string ExecuteQSeqment4  = @"QX4";
        internal const string ExecuteQSeqment5  = @"QX5";
        internal const string ExecuteQSeqment6  = @"QX6";
        internal const string ExecuteQSeqment7  = @"QX7";
        internal const string ExecuteQSeqment8  = @"QX8";
        internal const string ExecuteQSeqment9  = @"QX9";
        internal const string ExecuteQSeqment10 = @"QX10";
        internal const string ExecuteQSeqment11 = @"QX11";
        internal const string ExecuteQSeqment12 = @"QX12";


        //-----Encoder/Motor Counter---//


        //------Motor Status------//
        internal const string RquestAlarmCode       = @"AL";
        internal const string RquestImmediateFormat = @"IF";
        internal const string RquestStatus          = @"SC";
        internal const string RquestInputStatus     = @"IS";
        internal const string RquestOutputStatus    = @"IO";

        #endregion

        #region Constant for command set
        internal enum RemoteMotionCmdID
        {
            None = -1,
            AlarmCode,
            ImmediateFormat,
            StatusCode,
            InputStatusRequest,
            OutputStatus,
        }
        #endregion

        #region RemoteIO Command Send
        internal enum RMCommandSend
        {
            RemoteMotionConnect = 0,
        }
        #endregion

    }
}
