using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMWeb.Models
{
    public class UsuarioModel : GenericModel
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

        private string _nombre;

        public string Nombre
        {
            get { return _nombre; }
            set
            {
                _nombre = value;
                OnPropertyChange();
            }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChange();
            }
        }

        private List<EmpresaModel> empresaModels;

        public List<EmpresaModel> Empresas
        {
            get { return empresaModels; }
            set
            {
                empresaModels = value;
                OnPropertyChange();
            }
        }

        private List<ModuloModel> moduloModels;

        public List<ModuloModel> Modulos
        {
            get { return moduloModels; }
            set
            {
                moduloModels = value;
                OnPropertyChange();
            }
        }

        private List<IntSelectionModel> _tiposDocumentos;

        public List<IntSelectionModel> TiposDocumentos
        {
            get { return _tiposDocumentos; }
            set
            {
                _tiposDocumentos = value;
                OnPropertyChange();
            }
        }

        private List<DocumentoModel> _documentos;

        public List<DocumentoModel> Documentos
        {
            get { return _documentos; }
            set
            {
                _documentos = value;
                OnPropertyChange();
            }
        }

        private EmpresaModel _empresaSelected;

        public EmpresaModel EmpresaSelected
        {
            get { return _empresaSelected; }
            set
            {
                _empresaSelected = value;
                OnPropertyChange();
            }
        }

        private ModuloModel _moduloSelected;

        public ModuloModel ModuloSelected
        {
            get { return _moduloSelected; }
            set
            {
                _moduloSelected = value;
                OnPropertyChange();
            }
        }

        private IntSelectionModel intSelectionModel;

        public IntSelectionModel TipoDocumentoSelected
        {
            get { return intSelectionModel; }
            set
            {
                intSelectionModel = value;
                OnPropertyChange();
            }
        }

        private long _idDispositivo;

        public long IdDispositivo
        {
            get { return _idDispositivo; }
            set
            {
                _idDispositivo = value;
                OnPropertyChange();
            }
        }

        public UsuarioModel()
        {
            this.Id = 0;
            this.Nombre = string.Empty;
            this.Email = string.Empty;
            this.Empresas = new List<EmpresaModel>();
            this.Modulos = new List<ModuloModel>();
            this.TiposDocumentos = new List<IntSelectionModel>();
            this.Documentos = new List<DocumentoModel>();
            this.TiposDocumentos = new List<IntSelectionModel>
            {
                    new IntSelectionModel
                    {
                        Descripcion = "Negocios",
                        Valor = 1
                    },
                    new IntSelectionModel
                    {
                        Descripcion = "Tecnología",
                        Valor = 2
                    }
            };
            this.IdDispositivo = 0;

        }

    }
}
