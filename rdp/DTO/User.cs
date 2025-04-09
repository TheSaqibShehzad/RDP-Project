using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rdp.DTO
{
    public class User
    {
        public int FreeTweaks { get; set; }
        public bool IsPremiumUser { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Username { get; set; }
        public string Ip { get; set; }
        public string Hwid { get; set; }
        public string CreateDate { get; set; }
        public string LastLogin { get; set; }
        public string LicenseKey { get; set; }
        public bool IsActivated { get; set; }
    }
}
