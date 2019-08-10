using BCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMWeb.EventArgs
{
    public class LoginModelEventArgs : System.EventArgs
    {
        public UsuarioModel Usuario { get; set; }
        public string Token { get; set; }
    }
}
