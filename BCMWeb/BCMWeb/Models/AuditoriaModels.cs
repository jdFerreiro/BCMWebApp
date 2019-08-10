using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace BCMWeb.Models
{
    [Preserve(AllMembers = true)]
    public class AuditoriaModel
    {
        private string _errorMessge;

        [JsonProperty]
        public string ErrorMessage
        {
            get { return _errorMessge; }
            set { _errorMessge = value; }
        }


        private long _idUsuario;

        [JsonProperty]
        public long IdUsuario
        {
            get { return _idUsuario; }
            set { _idUsuario = value; }
        }



        private long _id;
        [JsonProperty]
        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

        private long _idEmpresa;

        [JsonProperty]
        public long IdEmpresa
        {
            get { return _idEmpresa; }
            set
            {
                _idEmpresa = value;
            }
        }

        private long _idDocumento;

        [JsonProperty]
        public long IdDocumento
        {
            get { return _idDocumento; }
            set
            {
                _idDocumento = value;
            }
        }

        private long _idTipoDocumento;

        [JsonProperty]
        public long IdTipoDocumento
        {
            get { return _idTipoDocumento; }
            set
            {
                _idTipoDocumento = value;
            }
        }

        private string _ipAddress;

        [JsonProperty]
        public string IpAddress
        {
            get { return _ipAddress; }
            set
            {
                _ipAddress = value;
            }
        }

        private string _mensaje;
        [JsonProperty]
        public string Mensaje
        {
            get { return _mensaje; }
            set
            {
                _mensaje = value;
            }
        }

        private string _accion;

        [JsonProperty]
        public string Accion
        {
            get { return _accion; }
            set
            {
                _accion = value;
            }
        }

        private bool _negocios;

        [JsonProperty]
        public bool Negocios
        {
            get { return _negocios; }
            set
            {
                _negocios = value;
            }
        }

        public async void GenerarRegistro(HttpClient httpClient, string userName, string errorMessage, long userId, string addressIP, string message, bool business)
        {

            AuditoriaModel _regAud = new AuditoriaModel()
            {
                Accion = string.Format("Accesa {0} vía móvil", userName),
                ErrorMessage = errorMessage,
                IdUsuario = userId,
                IpAddress = addressIP,
                Mensaje = message,
                Negocios = business,
            };
            var _cancelTokenSource = new CancellationTokenSource();
            var _cancelToken = _cancelTokenSource.Token;
            string serData = JsonConvert.SerializeObject(_regAud);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string auditoriaURL = "https://www.bcmweb.net/api/auditoria/add";

            var request = new HttpRequestMessage(new HttpMethod("POST"), auditoriaURL)
            {
                Content = new StringContent(serData, Encoding.UTF8, "application/json")
            };
            var result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(true);
        }
    }
}
