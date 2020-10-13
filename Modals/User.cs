using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modals
{
    public class User
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string deviceToken { get; set; }

        public string userType { get; set; }


    }
}
