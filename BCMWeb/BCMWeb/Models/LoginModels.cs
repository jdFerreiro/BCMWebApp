using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMWeb.Models
{
    public class LoginModel : GenericModel
    {
        private string _çodigo;

        public string Codigo
        {
            get { return _çodigo; }
            set
            {
                _çodigo = value;
                OnPropertyChange();
            }
        }

        private string _contraseña;

        public string Contraseña
        {
            get { return _contraseña; }
            set
            {
                _contraseña = value;
                OnPropertyChange();
            }
        }

        private string _strErrorCodigo;

        public string ErrorCodigo
        {
            get { return _strErrorCodigo; }
            set
            {
                _strErrorCodigo = value;
                OnPropertyChange();
            }
        }

        private string _strErrorPassw;

        public string ErrorPassw
        {
            get { return _strErrorPassw; }
            set
            {
                _strErrorPassw = value;
                OnPropertyChange();
            }
        }

        private string _errLogin;

        public string ErrorLogin
        {
            get { return _errLogin; }
            set
            {
                _errLogin = value;
                OnPropertyChange();
            }
        }

    }
}
