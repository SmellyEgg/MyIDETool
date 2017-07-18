using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xinLongIDE.Model.requestJson
{
    public class User 
    {
        private string username = string.Empty;

        private string pass_word = string.Empty;

        public User(string name, string psswd)
        {
            this.username = name;
            this.pass_word = psswd;
        }

        public string user_name
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
            }
        }

        public string password
        {
            get
            {
                return this.pass_word;
            }
            set
            {
                this.pass_word = value;
            }
        }
        //public string user_name { }
    }
}
