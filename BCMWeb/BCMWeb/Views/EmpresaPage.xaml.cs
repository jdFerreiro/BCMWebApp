using BCMWeb.Interfaces;
using BCMWeb.Models;
using Newtonsoft.Json;
using Plugin.DownloadManager;
using Plugin.DownloadManager.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BCMWeb.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmpresaPage : ContentPage
    {
        private UsuarioModel _usuarioActivo;
        private long IdEmpresa = 0;
        private string Token;
        private IDownloadFile File;
        private bool isDownloading = true;

        public EmpresaPage()
        {

            _usuarioActivo = (UsuarioModel)App.Current.Properties["user"];
            Token = App.Current.Properties["token"].ToString();

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    imgLogo.Source = ImageSource.FromFile("Images/Splash 40x40.png");
                    break;
                case Device.Android:
                    imgLogo.Source = ImageSource.FromFile("Splash_100x100.png");
                    break;
                case Device.UWP:
                    break;
            }

            string queryString = string.Format("https://www.bcmweb.net/api/empresa/getbyuser/{0}", _usuarioActivo.Id.ToString());
            _usuarioActivo = DataService.GetEmpresasUsuario(queryString, Token, _usuarioActivo).Result;
            BindingContext = _usuarioActivo;

            cmbEmpresa.SelectedIndexChanged += CmbEmpresa_SelectedIndexChangedAsync;
            cmbTipoDoc.SelectedIndexChanged += CmbTipoDoc_SelectedIndexChanged;
            cmbTipoDoc.ItemsSource = _usuarioActivo.TiposDocumentos;
            cmbTipoDoc.SelectedIndex = 0;
            cmbTipoDoc.SelectedItem = cmbTipoDoc.ItemsSource[cmbTipoDoc.SelectedIndex];

            if (_usuarioActivo.Empresas.Count == 1)
            {
                cmbEmpresa.SelectedIndex = 0;
                cmbEmpresa.IsEnabled = false;
            }
        }

        private async void CmbEmpresa_SelectedIndexChangedAsync(object sender, System.EventArgs e)
        {
            _usuarioActivo.IsBusy = true;
            //await GetEmpresa(sender, e);
            await GetEmpresa();
            cmbModulo.SelectedIndexChanged += CmbModulo_SelectedIndexChanged;
            _usuarioActivo.IsBusy = false;
        }
        private async void CmbTipoDoc_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            _usuarioActivo.IsBusy = true;
            await GetTipoDoc();
            _usuarioActivo.IsBusy = false;
        }
        private async void CmbModulo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            _usuarioActivo.IsBusy = true;
            await GetModulo();
            _usuarioActivo.IsBusy = false;
        }
        private async Task GetEmpresa()
        {
            _usuarioActivo.IsBusy = true;
            IdEmpresa = _usuarioActivo.EmpresaSelected.Id;
            string _queryString = string.Empty;
            if (_usuarioActivo.TipoDocumentoSelected.Valor == 1)
                _queryString = string.Format("https://www.bcmweb.net/api/modulo/GetPrincipalNegocioByEmpresa_Usuario/{0}/{1}", IdEmpresa.ToString(), _usuarioActivo.Id.ToString());
            else
                _queryString = string.Format("https://www.bcmweb.net/api/modulo/GetPrincipalTecnologiaByEmpresa_Usuario/{0}/{1}", IdEmpresa.ToString(), _usuarioActivo.Id.ToString());

            _usuarioActivo = await DataService.GetModulosUsuario(_queryString, Token, _usuarioActivo);
            AuditoriaModel _regAud = new AuditoriaModel
            {
                Accion = string.Format("Registro de conexion a la empresa {1}", _usuarioActivo.Nombre, _usuarioActivo.EmpresaSelected.Nombre),
                ErrorMessage = string.Empty,
                Id = 0,
                IdEmpresa = _usuarioActivo.EmpresaSelected.Id,
                IdUsuario = _usuarioActivo.Id,
                IpAddress = DependencyService.Get<IIPAddressManager>().GetIpAddress(),
                Mensaje = String.Empty,
                Negocios = true,
            };
            bool _done = await DataService.RegistrarAuditoria(_regAud, Token).ConfigureAwait(true);

            _usuarioActivo.IsBusy = false;
        }
        private async Task GetModulo() //object sender, System.EventArgs e)
        {
            _usuarioActivo.IsBusy = true;
            if (cmbEmpresa.SelectedItem != null && cmbModulo.SelectedItem != null && cmbTipoDoc.SelectedItem != null)
            {
                _usuarioActivo.Documentos = new List<DocumentoModel>();
                EmpresaModel empresa = (EmpresaModel)cmbEmpresa.SelectedItem;
                ModuloModel modulo = (ModuloModel)cmbModulo.SelectedItem;
                IntSelectionModel Clase = (IntSelectionModel)cmbTipoDoc.SelectedItem;
                await GetDocumentos(empresa.Id, _usuarioActivo.Id, Clase.Valor, modulo.IdCodigoModulo);
            }
            _usuarioActivo.IsBusy = false;
        }
        private async Task GetTipoDoc() //object sender, System.EventArgs e)
        {
            _usuarioActivo.IsBusy = true;
            if (cmbEmpresa.SelectedItem != null && cmbModulo.SelectedItem != null && cmbTipoDoc.SelectedItem != null)
            {
                _usuarioActivo.Documentos = new List<DocumentoModel>();
                EmpresaModel empresa = (EmpresaModel)cmbEmpresa.SelectedItem;
                ModuloModel modulo = (ModuloModel)cmbModulo.SelectedItem;
                IntSelectionModel Clase = (IntSelectionModel)cmbTipoDoc.SelectedItem;
                await GetDocumentos(empresa.Id, _usuarioActivo.Id, Clase.Valor, modulo.IdCodigoModulo);
            }
            _usuarioActivo.IsBusy = false;
        }
        private async Task GetDocumentos(long IdEmpresa, long IdUsuario, int IdClase, long IdTipo)
        {

            _usuarioActivo.IsBusy = true;
            string _queryString = string.Format("https://www.bcmweb.net/api/documento/getbytypeclass/{0}/{1}/{2}/{3}", IdUsuario.ToString(), IdEmpresa.ToString(), IdClase.ToString(), IdTipo.ToString());
            _usuarioActivo = await DataService.GetDocumentosUsuario(_queryString, Token, _usuarioActivo);

            grdBotones.Children.Clear();

            if (_usuarioActivo.Documentos.Count > 0)
            {
                int _Rows = _usuarioActivo.Documentos.Count / 3;
                if (_Rows != (_usuarioActivo.Documentos.Count / 3))
                    _Rows += 1;

                grdBotones.ColumnDefinitions.Clear();
                grdBotones.RowDefinitions.Clear();
                grdBotones.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grdBotones.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grdBotones.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                for (int i = 0; i < _Rows; i++)
                {
                    grdBotones.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                }

                int _actualRow = 0;
                int _actualCol = -1;
                foreach (DocumentoModel _documento in _usuarioActivo.Documentos)
                {
                    _actualCol += 1;
                    if (_actualCol == 3)
                    {
                        _actualRow++;
                        _actualCol = 0;
                    }

                    string _nombreDoc = string.Format("{0} {3} {1}.{2}", _documento.TipoDocumento, _documento.VersionOriginal.ToString(), _documento.NroVersion.ToString(), _documento.NombreDocumento);
                    string[] parameters = {
                        _documento.PdfRoute,
                        _nombreDoc,
                        _usuarioActivo.EmpresaSelected.Id.ToString(),
                        _usuarioActivo.Id.ToString(),
                        _documento.Id.ToString(),
                        _documento.IdTipoDocumento.ToString(),
                        _documento.Negocios.ToString(),

                    };
                    TapGestureRecognizer _tgrDoc = new TapGestureRecognizer();
                    _tgrDoc.Tapped += _tgrDoc_Tapped;
                    _tgrDoc.CommandParameter = parameters;
                    _tgrDoc.StyleId = _nombreDoc;

                    StackLayout _SLButton = new StackLayout();
                    _SLButton.GestureRecognizers.Add(_tgrDoc);
                    Grid.SetRow(_SLButton, _actualRow);
                    Grid.SetColumn(_SLButton, _actualCol);

                    StackLayout _SLButtonInternal = new StackLayout
                    {

                        Orientation = StackOrientation.Vertical,
                        Spacing = 5,
                        Padding = 10,
                        BackgroundColor = Color.FromHex("#ffE3E3E3"),
                        WidthRequest = 85,
                    };

                    StackLayout _SLInternalStart = new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                    };
                    StackLayout _SLInternalEnd = new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                    };
                    Label _LabelButton = new Label
                    {
                        BackgroundColor = Color.FromHex("#ffE3E3E3"),
                        Text = _nombreDoc,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                    };
                    Image _imgButton = new Image
                    {
                        BackgroundColor = Color.FromHex("#ffE3E3E3"),
                        WidthRequest = 40,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                    };
                    switch (Device.RuntimePlatform)
                    {
                        case Device.iOS:
                            break;
                        case Device.Android:
                            _imgButton.Source = ImageSource.FromFile("icono_pdf_def.jpg");
                            break;
                        case Device.UWP:
                            break;
                    }

                    _SLButtonInternal.Children.Add(_SLInternalStart);
                    _SLButtonInternal.Children.Add(_imgButton);
                    _SLButtonInternal.Children.Add(_LabelButton);
                    _SLButtonInternal.Children.Add(_SLInternalEnd);
                    _SLButton.Children.Add(_SLButtonInternal);
                    grdBotones.Children.Add(_SLButton);
                }
            }
            _usuarioActivo.IsBusy = true;
        }
        private async void _tgrDoc_Tapped(object sender, System.EventArgs e)
        {
            try
            {
                _usuarioActivo.IsBusy = true;
                StackLayout _slButton = (StackLayout)sender;
                TapGestureRecognizer _tgrDoc = (TapGestureRecognizer)_slButton.GestureRecognizers[0];
                string _dialogMessage = string.Format("Se descargará el archivo {0} al dispositivo. ¿Desea continuar?", _tgrDoc.StyleId);
                _usuarioActivo.IsBusy = false;
                var answer = await DisplayAlert("Descargar archivo", _dialogMessage, "Yes", "No");
                string _message = string.Empty;
                if (answer)
                {
                    _usuarioActivo.IsBusy = true;
                    string[] _parameters = _tgrDoc.CommandParameter as string[];
                    string _result = string.Empty;

                    //_result = await DataService.DownloadDocument(_parameters[0], Token, _tgrDoc.StyleId, _parameters);
                    string _urlPath = _parameters[0].Replace("C:\\BCMWEB", "https://www.bcmweb.net").Replace("\\", "/");

                    DownloadFile(_urlPath, _tgrDoc.StyleId, _parameters);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region DownloadFiles
        public async void DownloadFile(string FileRoot, string FileName, string[] _parameters)
        {
            try
            {
                string _downloadStatus = string.Empty;

                await Task.Run(() =>
                {
                    var downloadManager = CrossDownloadManager.Current;
                    File = downloadManager.CreateDownloadFile(FileRoot);
                    downloadManager.Start(File, true);

                    while (isDownloading)
                    {
                        isDownloading = IsDownloading(File, FileName, _parameters).Result;
                    }
                }).ConfigureAwait(true);

                if (!isDownloading)
                {
                    string _title = "Descarga de documentos";
                    string _body = string.Format("Archivo {0} Descargado satisfactoriamente, revise su carpeta de descargas", FileName);
                    string _bodyNotify = string.Format("{0} Descargado.", FileName);

                    //CrossLocalNotifications.Current.Show(_title, _bodyNotify, 100, DateTime.Now.AddSeconds(1));
                    _usuarioActivo.IsBusy = false;
                    await DisplayAlert(_title, _body, "OK");
                }
            }
            catch (Exception ex)
            {
                var _message = ex.Message;
                throw ex;
            }
        }

        public void AbortDownloading()
        {
            CrossDownloadManager.Current.Abort(File);
        }
        public async Task<bool> IsDownloading(IDownloadFile File, string Filename, string[] _parameters)
        {
            if (File == null)
                return false;

            bool _returnValue;
            string _accion = string.Empty;

            switch (File.Status)
            {
                case DownloadFileStatus.INITIALIZED:
                    _accion = string.Format("La descarga del documento {0} se ha inicializado", Filename);
                    _returnValue = true;
                    break;
                case DownloadFileStatus.PAUSED:
                    _accion = string.Format("La descarga del documento {0} se encuentra en pausa", Filename);
                    _returnValue = true;
                    break;
                case DownloadFileStatus.PENDING:
                    _accion = string.Format("La descarga del documento {0} se encuentra pendiente", Filename);
                    _returnValue = true;
                    break;
                case DownloadFileStatus.RUNNING:
                    _accion = string.Format("La descarga del documento {0} se encuentra en ejecución", Filename);
                    _returnValue = true;
                    break;
                case DownloadFileStatus.COMPLETED:
                    _accion = string.Format("La descarga del documento {0} se ha completado", Filename);
                    _returnValue = false;
                    break;
                case DownloadFileStatus.CANCELED:
                    _accion = string.Format("La descarga del documento {0} se ha cancelado", Filename);
                    _returnValue = false;
                    break;
                case DownloadFileStatus.FAILED:
                    _accion = string.Format("La descarga del documento {0} ha fallado", Filename);
                    _returnValue = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            long _idDocumento = long.Parse(_parameters[4]);
            long _idTipoDocumento = long.Parse(_parameters[5]);
            bool _negocios = bool.Parse(_parameters[6]);

            AuditoriaModel _regAud = new AuditoriaModel
            {
                Accion = _accion,
                ErrorMessage = string.Empty,
                Id = 0,
                IdEmpresa = _usuarioActivo.EmpresaSelected.Id,
                IdDocumento = _idDocumento,
                IdTipoDocumento = _idTipoDocumento,
                IdUsuario = _usuarioActivo.Id,
                IpAddress = DependencyService.Get<IIPAddressManager>().GetIpAddress(),
                Mensaje = String.Empty,
                Negocios = _negocios,
            };
            bool _done = await DataService.RegistrarAuditoria(_regAud, Token).ConfigureAwait(true);

            return _returnValue;
        }
        #endregion

        private async void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new LoginPage());
            var Pages = Navigation.NavigationStack.ToList();
            foreach (var page in Pages)
            {
                if (page.GetType() != typeof(LoginPage))
                {
                    Navigation.RemovePage(page);
                }
            }
        }
    }
}