using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Models
{
    public class EmailSettings
    {        
        public string SMTPHost { get; set; }
        public int SMTPPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
