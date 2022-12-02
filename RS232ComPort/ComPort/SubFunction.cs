using System;
using System.IO.Ports;
using System.Text;

namespace RS232ComPort.ComPort
{
    public class SubFunction
    {
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        public static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }

        public static byte GetCheckSumByte(byte[] cmd)
        {
            byte checkSumByte = 0x00;
            for (int i = 0; i < cmd.Length; i++)
                checkSumByte ^= cmd[i];
            return checkSumByte;
        }

        public static string[] ScanComPort()
        {
            string[] ports = SerialPort.GetPortNames();
            return ports;
        }
    }
}
