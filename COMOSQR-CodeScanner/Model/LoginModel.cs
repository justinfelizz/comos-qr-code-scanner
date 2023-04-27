using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace COMOSQR_CodeScanner.Model
{
    internal class LoginModel
    {
        public string Webserver { get; set; }
        public string Username { get; set; }
        public ICommand DoLogin { get; set; }
    }
}
