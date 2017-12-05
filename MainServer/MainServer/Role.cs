using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer
{
     public class Role
    {
        private string login;
        private string password;
        public string Login()
        {
            return login;
        }
        public string Password()
        {
            return password;
        }
        public Role(string login,string password)
        {
            this.login = login;
            this.password = password;
        }
    }
}
