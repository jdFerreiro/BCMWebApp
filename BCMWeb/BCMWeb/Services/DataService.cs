using BCMWeb.Interfaces;
using BCMWeb.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BCMWeb
{
    public static class DataService
    {

        //[Android.Runtime.Register("getExternalStoragePublicDirectory", "(Ljava/lang/String;)Ljava/io/File;", "")]
        //public static File GetExternalStoragePublicDirectory(String type);

        public static async Task<TokenModel> GetToken(string userName, string password)
        {
            string url = "https://www.bcmweb.net/token";
            TokenModel _token = new TokenModel();

            using (HttpClient httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var param = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "username", userName },
                    { "password", password }
                };

                var request = new HttpRequestMessage(new HttpMethod("POST"), url)
                {
                    Content = new FormUrlEncodedContent(param)
                };

                try
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(60);
                    var _cancelTokenSource = new CancellationTokenSource();
                    var _cancelToken = _cancelTokenSource.Token;
                    var result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(true);
                    if (result.IsSuccessStatusCode)
                    {
                        string resultContent = await result.Content.ReadAsStringAsync().ConfigureAwait(true);
                        _token = JsonConvert.DeserializeObject<TokenModel>(resultContent);
                        var _accessToken = JObject.Parse(resultContent)["access_token"];
                        var jsonToken = JsonConvert.DeserializeObject<TokenModel>(resultContent);
                        _token.ErrorMessage = string.Empty;
                    }
                    else
                    {
                        _token.ErrorMessage = "Credenciales inválidas";
                    }
                }
                catch (TaskCanceledException tex)
                {
                    _token.ErrorMessage = tex.Message;
                }
                catch (Exception ex)
                {
                    _token.ErrorMessage = ex.Message;
                    _token.ErrorMessage = "No se pudieron verificar las credenciales. Verifique su acceso a internet";
                }
            }

            return _token;
        }
        public static async Task<UsuarioModel> GetdataUsuario(string queryString, string _token)
        {
            UsuarioModel _usuario = new UsuarioModel();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    httpClient.Timeout = TimeSpan.FromSeconds(60);
                    var _cancelTokenSource = new CancellationTokenSource();
                    var _cancelToken = _cancelTokenSource.Token;
                    var request = new HttpRequestMessage(new HttpMethod("GET"), queryString);
                    var result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(true);
                    if (result.IsSuccessStatusCode)
                    {
                        string json = result.Content.ReadAsStringAsync().Result;
                        _usuario = JsonConvert.DeserializeObject<UsuarioModel>(json);
                        _usuario.ErrorMessage = string.Empty;

                        long IdUsuario = _usuario.Id;
                        string _IpAddress = DependencyService.Get<IIPAddressManager>().GetIpAddress();

                        AuditoriaModel _auditModel = new AuditoriaModel();
                        _auditModel.GenerarRegistro(httpClient, _usuario.Nombre, string.Empty, IdUsuario, _IpAddress, string.Empty, true);

                        /* *******************************
                         * Obtener datos del dispositivo *
                         * *******************************/

                        DispositivoModel Dispositivo = DependencyService.Get<IIDeviceManager>().GetDeviceData();
                        Dispositivo.FechaRegistro = DateTime.Now;
                        Dispositivo.IdUsuario = _usuario.Id;

                        string _verifyDispUrl = string.Format("https://www.bcmweb.net/api/device/ExistDevice/{0}", Dispositivo.IdUnicoDispositivo);
                        request = new HttpRequestMessage(new HttpMethod("GET"), _verifyDispUrl);
                        result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(true);
                        if (result.IsSuccessStatusCode)
                        {
                            json = result.Content.ReadAsStringAsync().Result;
                            long dispId = JsonConvert.DeserializeObject<long>(json);

                            if (dispId == 0)
                            {

                                string serDataDisp = JsonConvert.SerializeObject(Dispositivo);
                                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                string addDispositvoURL = "https://www.bcmweb.net/api/device/addDevice";

                                request = new HttpRequestMessage(new HttpMethod("POST"), addDispositvoURL)
                                {
                                    Content = new StringContent(serDataDisp, Encoding.UTF8, "application/json")
                                };
                                result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(true);
                                if (result.IsSuccessStatusCode)
                                {
                                    json = result.Content.ReadAsStringAsync().Result;
                                    Dispositivo = JsonConvert.DeserializeObject<DispositivoModel>(json);
                                    _usuario.IdDispositivo = Dispositivo.Id;
                                }

                            }
                            else
                            {
                                _usuario.IdDispositivo = dispId;
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                _usuario.ErrorMessage = ex.Message;
            }

            return _usuario;

        }
    
        public static async Task<UsuarioModel> GetEmpresasUsuario(string queryString, string _token, UsuarioModel model)
        {
            List<EmpresaModel> _empresas = new List<EmpresaModel>();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    httpClient.DefaultRequestHeaders.TransferEncodingChunked = true;
                    var _cancelTokenSource = new CancellationTokenSource();
                    var _cancelToken = _cancelTokenSource.Token;
                    var request = new HttpRequestMessage(HttpMethod.Get, queryString);
                    try
                    {
                        var result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(false);
                        if (result.IsSuccessStatusCode)
                        {
                            string json = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                            
                            _empresas = JsonConvert.DeserializeObject<List<EmpresaModel>>(json);
                        }
                    }
                    catch (Exception ex)
                    {
                        model.ErrorMessage = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
            }

            model.Empresas = _empresas;
            return model;
        }
        public static async Task<UsuarioModel> GetModulosUsuario(string queryString, string _token, UsuarioModel model)
        {
            try
            {
                string[] _querySplit = queryString.Split('/');
                long IdEmpresa = long.Parse(_querySplit[_querySplit.Length - 2]);

                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    var _cancelTokenSource = new CancellationTokenSource();
                    var _cancelToken = _cancelTokenSource.Token;
                    var request = new HttpRequestMessage(new HttpMethod("GET"), queryString);
                    var result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(true);
                    if (result.IsSuccessStatusCode)
                    {
                        string json = result.Content.ReadAsStringAsync().Result;
                        model.Modulos = JsonConvert.DeserializeObject<List<ModuloModel>>(json);

                        ConexionDispositivoModel _conexion = new ConexionDispositivoModel
                        {
                            DireccionIP = DependencyService.Get<IIPAddressManager>().GetIpAddress(),
                            FechaConexion = DateTime.Now,
                            IdDispositivo = model.IdDispositivo,
                            IdEmpresa = IdEmpresa,
                            IdUsuario = model.Id,
                        };
                        string serDataDisp = JsonConvert.SerializeObject(_conexion);
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string addDispositvoURL = "https://www.bcmweb.net/api/device/addConexion";

                        request = new HttpRequestMessage(new HttpMethod("POST"), addDispositvoURL)
                        {
                            Content = new StringContent(serDataDisp, Encoding.UTF8, "application/json")
                        };
                        result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(true);
                        AuditoriaModel _regAud = new AuditoriaModel
                        {
                            Accion = string.Format("Visualiza documentos de la empresa {0}, del módulo {1}", model.EmpresaSelected.Nombre, model.ModuloSelected.Nombre),
                            ErrorMessage = string.Empty,
                            Id = 0,
                            IdEmpresa = model.EmpresaSelected.Id,
                            IdUsuario = model.Id,
                            IpAddress = DependencyService.Get<IIPAddressManager>().GetIpAddress(),
                            Mensaje = String.Empty,
                            Negocios = true,
                        };
                        bool _done = await DataService.RegistrarAuditoria(_regAud, _token).ConfigureAwait(true);

                    }

                }
            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
            }

            return model;
        }
        public static async Task<UsuarioModel> GetDocumentosUsuario(string queryString, string _token, UsuarioModel model)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    var _cancelTokenSource = new CancellationTokenSource();
                    var _cancelToken = _cancelTokenSource.Token;
                    var request = new HttpRequestMessage(new HttpMethod("GET"), queryString);
                    var result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(true);
                    if (result.IsSuccessStatusCode)
                    {
                        string json = result.Content.ReadAsStringAsync().Result;
                        model.Documentos = JsonConvert.DeserializeObject<List<DocumentoModel>>(json);
                    }

                    long IdUsuario = long.Parse(queryString.Split('/')[6]);

                    AuditoriaModel _regAud = new AuditoriaModel
                    {
                        Accion = string.Format("Visualiza documentos de la empresa {0}, del módulo {1}", model.EmpresaSelected.Nombre, model.ModuloSelected.Nombre),
                        ErrorMessage = string.Empty,
                        Id = 0,
                        IdEmpresa = model.EmpresaSelected.Id,
                        IdUsuario = model.Id,
                        IpAddress = DependencyService.Get<IIPAddressManager>().GetIpAddress(),
                        Mensaje = String.Empty,
                        Negocios = true,
                    };
                    bool _done = await RegistrarAuditoria(_regAud, _token).ConfigureAwait(true);

                }

            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
            }

            return model;
        }
        public static async Task<string> DownloadDocument(string docPath, string _token, AuditoriaModel _regAud)
        {

            string _messageResult = string.Empty;
            string _urlPath = docPath.Replace("C:\\BCMWEB", "https://www.bcmweb.net").Replace("\\", "/");
            int _startPos = docPath.LastIndexOf("\\");
            string _docName = docPath.Substring(_startPos + 1);

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    httpClient.Timeout = TimeSpan.FromMinutes(30);
                    var _cancelTokenSource = new CancellationTokenSource();
                    var _cancelToken = _cancelTokenSource.Token;
                    var request = new HttpRequestMessage(new HttpMethod("GET"), _urlPath);
                    var result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(true);
                    if (result.IsSuccessStatusCode)
                    {
                        string _folderPath = DependencyService.Get<IIDownloadPathService>().GetDownloadFolder();
                        PCLStorage.IFolder _destFolder = await PCLStorage.FileSystem.Current.GetFolderFromPathAsync(_folderPath);

                        //IFolder _folder = await _destFolder.CreateFolderAsync("BCMWebDocs", CreationCollisionOption.OpenIfExists);
                        PCLStorage.IFile _file = await _destFolder.CreateFileAsync(_docName, PCLStorage.CreationCollisionOption.ReplaceExisting);

                        using (var fileHandler = await _file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite, default(CancellationToken)))
                        {
                            byte[] _fileBuffer = await result.Content.ReadAsByteArrayAsync();
                            await fileHandler.WriteAsync(_fileBuffer, 0, _fileBuffer.Length);
                        }

                        string serData = JsonConvert.SerializeObject(_regAud);
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        string auditoriaURL = "https://www.bcmweb.net/api/auditoria/add";

                        request = new HttpRequestMessage(new HttpMethod("POST"), auditoriaURL)
                        {
                            Content = new StringContent(serData, Encoding.UTF8, "application/json")
                        };
                        result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(true);

                        _messageResult = string.Format("El documento fue descargado satisfactoriamente, y ubicado en \"{0}\"", _folderPath);
                    }

                }
            }
            catch (TaskCanceledException tex)
            {
                if (!tex.CancellationToken.IsCancellationRequested)
                {
                    _messageResult = "La conexión a internet no permite descargar el archivo";
                }
            }
            catch (Exception ex)
            {
                string _msg = ex.Message;
                if (ex.Message.ToLowerInvariant().Contains("instance"))
                {
                    //_messageResult = "Error descargando el documento. Notifique a Soporte";
                    _messageResult = ex.Message;
                }
                else if (ex.Message.ToLowerInvariant().Contains("task"))
                {
                    _messageResult = "No se pudo descargar el documento. Verifique su acceso a internet";
                }
                else
                {
                    ex.Message.ToLowerInvariant().Contains("instance");
                }
            }

            return _messageResult;
        }
        public static async Task<List<DocumentoPendienteModel>> GetDocsPendings(string deviceId, string _token, UsuarioModel model)
        {
            List<DocumentoPendienteModel> _documentos = new List<DocumentoPendienteModel>();

            try
            {
                string queryString = string.Format("https://www.bcmweb.net/api/Device/GetDocsDevice/{0}", deviceId);

                using (HttpClient httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
                    httpClient.Timeout = TimeSpan.FromSeconds(60);
                    var _cancelTokenSource = new CancellationTokenSource();
                    var _cancelToken = _cancelTokenSource.Token;
                    var request = new HttpRequestMessage(new HttpMethod("GET"), queryString);
                    var result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(true);
                    if (result.IsSuccessStatusCode)
                    {
                        string json = result.Content.ReadAsStringAsync().Result;
                        _documentos = JsonConvert.DeserializeObject<List<DocumentoPendienteModel>>(json);
                    }

                    long IdUsuario = long.Parse(queryString.Split('/')[6]);

                    AuditoriaModel _regAud = new AuditoriaModel
                    {
                        Accion = string.Format("Obtiene documentos pendientes para el dipositivo {0}", deviceId),
                        ErrorMessage = string.Empty,
                        Id = 0,
                        IdEmpresa = model.EmpresaSelected.Id,
                        IdUsuario = model.Id,
                        IpAddress = DependencyService.Get<IIPAddressManager>().GetIpAddress(),
                        Mensaje = String.Empty,
                        Negocios = true,
                    };
                    string serData = JsonConvert.SerializeObject(_regAud);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string auditoriaURL = "https://www.bcmweb.net/api/auditoria/add";

                    request = new HttpRequestMessage(new HttpMethod("POST"), auditoriaURL)
                    {
                        Content = new StringContent(serData, Encoding.UTF8, "application/json")
                    };
                    result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(true);

                }
            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
            }

            return _documentos;
        }
        public static async Task<bool> RegistrarAuditoria(AuditoriaModel regAud, string token)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    var _cancelTokenSource = new CancellationTokenSource();
                    var _cancelToken = _cancelTokenSource.Token;

                    string serData = JsonConvert.SerializeObject(regAud);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string auditoriaURL = "https://www.bcmweb.net/api/auditoria/add";

                    var request = new HttpRequestMessage(new HttpMethod("POST"), auditoriaURL)
                    {
                        Content = new StringContent(serData, Encoding.UTF8, "application/json")
                    };
                    var result = await httpClient.SendAsync(request, _cancelToken).ConfigureAwait(true);

                    if (result.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                var _message = ex.Message;
                throw ex;
            }
        }
    }
}
