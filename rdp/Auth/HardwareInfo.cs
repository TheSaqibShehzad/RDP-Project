using rdp.Auth;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace rdp.Auth
{
    public static class HardwareInfo
    {
        public static string GetOSId()
        {
            var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_OperatingSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["SerialNumber"].ToString();
            }
            return "Unknown";
        }

        public static string GetMotherboardId()
        {
            var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");
            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["SerialNumber"].ToString();
            }
            return "Unknown";
        }

        public static string GetBiosSerialNumber()
        {
            var searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS");
            foreach (ManagementObject obj in searcher.Get())
            {
                return obj["SerialNumber"].ToString();
            }
            return "Unknown";
        }

        public static string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }
                return hashString.ToString();
            }
        }

        public static string GetAuthkey()
        {
            string osId = GetOSId();
            string motherboardId = GetMotherboardId();
            string biosId = GetBiosSerialNumber();
            //string authKey = $"{osId}+{motherboardId}+{biosId}";
            string authKey = $"{motherboardId}+{biosId}";
            string encodedAuthkey = ComputeSHA256Hash(authKey);
            return encodedAuthkey;
        }



    }
}