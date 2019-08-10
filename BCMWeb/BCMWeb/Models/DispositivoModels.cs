using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMWeb.Models
{
    public class DispositivoModel : GenericModel
    {
        private long _id;
        private DateTime _fechaRegistro;
        private string _idUnico;
        private string _fabricante;
        private string _modelo;
        private string _plataforma;
        private string _version;
        private string _nombre;
        private string _tipo;

        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChange();
            }
        }
 
       public DateTime FechaRegistro
        {
            get { return _fechaRegistro; }
            set
            {
                _fechaRegistro = value;
                OnPropertyChange();
            }
        }
        public string IdUnicoDispositivo
        {
            get { return _idUnico; }
            set
            {
                _idUnico = value;
                OnPropertyChange();
            }
        }
        public string Fabricante
        {
            get { return _fabricante; }
            set
            {
                _fabricante = value;
                OnPropertyChange();
            }
        }
        public string Modelo
        {
            get { return _modelo; }
            set
            {
                _modelo = value;
                OnPropertyChange();
            }
        }
        public string Plataforma
        {
            get { return _plataforma; }
            set
            {
                _plataforma = value;
                OnPropertyChange();
            }
        }
        
        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                OnPropertyChange();
            }
        }
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                _nombre = value;
                OnPropertyChange();
            }
        }
        public string Tipo
        {
            get { return _tipo; }
            set
            {
                _tipo = value;
                OnPropertyChange();
            }
        }

    }
    public class ConexionDispositivoModel : GenericModel
    {
        private long _idEmpresa;
        private long _idDispositivo;
        private DateTime _fechaConexion;
        private string _direccionIP;

        public long IdEmpresa
        {
            get { return _idEmpresa; }
            set
            {
                _idEmpresa = value;
                OnPropertyChange();
            }
        }
        public long IdDispositivo
        {
            get { return _idDispositivo; }
            set
            {
                _idDispositivo = value;
                OnPropertyChange();
            }
        }
        public DateTime FechaConexion
        {
            get { return _fechaConexion; }
            set
            {
                _fechaConexion = value;
                OnPropertyChange();
            }
        }
        public string DireccionIP
        {
            get { return _direccionIP; }
            set
            {
                _direccionIP = value;
                OnPropertyChange();
            }
        }

    }
}
