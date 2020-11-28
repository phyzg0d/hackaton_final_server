using System;
using System.Linq;

namespace ServerAspNetCoreLinux.ServerCore
{
    public static class StringHelper
    {
        public static string ToBinaryString(byte[] data)
        {
            return string.Join("", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
        }
        
        public static byte[] ToByteArray(string binaryString)
        {
            int numOfBytes = binaryString.Length / 8;
            byte[] bytes = new byte[numOfBytes];
            for (int i = 0; i < numOfBytes; i++)
            {
                string oneBinaryByte = binaryString.Substring(8 * i, 8);
                bytes[i] = Convert.ToByte(oneBinaryByte, 2);
            }
            return bytes;
        }
    }
}