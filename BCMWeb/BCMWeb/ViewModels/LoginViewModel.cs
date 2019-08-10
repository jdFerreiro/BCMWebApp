using BCMWeb.EventArgs;
using BCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BCMWeb.ViewModels
{
    public class LoginViewModel : LoginModel
    {

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await Login(), () => !IsBusy);
            ClearCommand = new Command(Clear, () => !IsBusy);
        }

        public Command LoginCommand { get; set; }
        public Command ClearCommand { get; set; }

        public event EventHandler<LoginModelEventArgs> UserLogged;

        private async Task Login()
        {
            this.IsBusy = true;
            if (ValidateData())
                await LoginUser();
            this.IsBusy = false;
        }

        private bool ValidateData()
        {
            bool _isValid = true;
            string message = string.Empty;
            ErrorCodigo = String.Empty;
            ErrorPassw = string.Empty;
            ErrorMessage = string.Empty;
            ErrorLogin = string.Empty;

            if (string.IsNullOrEmpty(Codigo))
            {
                ErrorCodigo = "Indique código de acceso";
                _isValid = false;
            }

            if (string.IsNullOrEmpty(Contraseña))
            {
                ErrorPassw = "Indique contraseña";
                _isValid = false;
            }

            return _isValid;
        }

        private async Task LoginUser()
        {
            TokenModel model = await DataService.GetToken(Codigo, Contraseña);
            if (!string.IsNullOrEmpty(model.ErrorMessage))
            {
                if (model.ErrorMessage.ToLowerInvariant().Contains("task"))
                {
                    model.ErrorMessage = "Tiempo de espera excedido. Verifique su conexión a internet e intente nuevamente.";
                }
                ErrorLogin = model.ErrorMessage;
                return;
            }
            string _urlUsuario = string.Format("https://www.bcmweb.net/api/Usuario/GetByCredentials/{0}/{1}", Codigo, Contraseña);
            UsuarioModel usuarioModel = await DataService.GetdataUsuario(_urlUsuario, model.access_token);
            SharedModel.UserToken = model.access_token;

            LoginModelEventArgs args = new LoginModelEventArgs
            {
                Token = model.access_token,
                Usuario = usuarioModel,
            };
            UserLogged?.Invoke(this, args);
        }

        private void Clear()
        {
            SharedModel.UserToken = string.Empty;
            Codigo = string.Empty;
            Contraseña = string.Empty;
            ErrorCodigo = String.Empty;
            ErrorPassw = string.Empty;
            ErrorMessage = string.Empty;
            ErrorLogin = string.Empty;
            IsBusy = false;
        }
    }
}
