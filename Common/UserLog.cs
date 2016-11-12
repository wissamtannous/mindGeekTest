using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    public class UserLog
    {
        public Guid GameID { get; set; }
        public string UserIP { get; set; }
        public DateTime Time { get; set; }
        public string Country { get; set; }
        public string UserAgent { get; set; }
        public string HeaderData { get; set; }
    }
}