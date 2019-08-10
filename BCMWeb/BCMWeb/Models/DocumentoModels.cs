using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMWeb.Models
{
    public class DocumentoModel : GenericModel
    {
        private long _id;

        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChange();
            }
        }

        private string _codigo;

        public string Codigo
        {
            get { return _codigo; }
            set
            {
                _codigo = value;
                OnPropertyChange();
            }
        }

        private long _idTipoDocumento;

        public long IdTipoDocumento
        {
            get { return _idTipoDocumento; }
            set
            {
                _idTipoDocumento = value;
                OnPropertyChange();
            }
        }

        private bool _negocios;

        public bool Negocios
        {
            get { return _negocios; }
            set
            {
                _negocios = value;
                OnPropertyChange();
            }
        }

        private long _nroDocumento;

        public long NroDocumento
        {
            get { return _nroDocumento; }
            set
            {
                _nroDocumento = value;
                OnPropertyChange();
            }
        }

        private int _nroVersion;

        public int NroVersion
        {
            get { return _nroVersion; }
            set
            {
                _nroVersion = value;
                OnPropertyChange();
            }
        }

        private string _pdfRoute;

        public string PdfRoute
        {
            get { return _pdfRoute; }
            set
            {
                _pdfRoute = value;
                OnPropertyChange();
            }
        }

        private string _tipoDocumento;

        public string TipoDocumento
        {
            get { return _tipoDocumento; }
            set
            {
                _tipoDocumento = value;
                OnPropertyChange();
            }
        }

        private int _versionOriginal;

        public int VersionOriginal
        {
            get { return _versionOriginal; }
            set
            {
                _versionOriginal = value;
                OnPropertyChange();
            }
        }

        private string _nombreDocumento;

        public string NombreDocumento
        {
            get { return _nombreDocumento; }
            set
            {
                _nombreDocumento = value;
                OnPropertyChange();
            }
        }


    }
    public class DocumentosPendientesModel : GenericModel
    {
        private List<DocumentoPendienteModel> _documentos;

        public List<DocumentoPendienteModel> Documentos
        {
            get { return _documentos; }
            set
            {
                _documentos = value;
                OnPropertyChange();
            }
        }

    }
    public class DocumentoPendienteModel : GenericModel
    {
        private long _idDocumento;
        public long IdDocumento
        {
            get { return _idDocumento; }
            set
            {
                _idDocumento = value;
                OnPropertyChange();
            }

        }

        private long _idEmpresa;
        public long IdEmpresa
        {
            get { return _idEmpresa; }
            set
            {
                _idEmpresa = value;
                OnPropertyChange();
            }

        }

        private string _nombreDocumento;
        public string NombreDocumento
        {
            get { return _nombreDocumento; }
            set
            {
                _nombreDocumento = value;
                OnPropertyChange();
            }
        }

        private string _rutaDocumento;
        public string RutaDocumento
        {
            get { return _rutaDocumento; }
            set
            {
                _rutaDocumento = value;
                OnPropertyChange();
            }
        }

    }
}
