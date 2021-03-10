using System;
using System.Collections.Generic;
using System.Text;

namespace APP_GACH_NO.Models
{
    public class User
    {
        public string MATNGAN { get; set; }
        public string THUNGAN { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string TOKEN { get; set; }
        public DateTime NGAYHHLUC { get; set; }
    }
}
