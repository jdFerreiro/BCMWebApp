using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMWeb.Models
{
    public class ModuloModel : GenericModel
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

        private long _idPadre;

        public long IdPadre
        {
            get { return _idPadre; }
            set
            {
                _idPadre = value;
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

        private int _idCodigoModulo;

        public int IdCodigoModulo
        {
            get { return _idCodigoModulo; }
            set
            {
                _idCodigoModulo = value;
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

        private bool _tecnologia;

        public bool Tecnologia
        {
            get { return _tecnologia; }
            set
            {
                _tecnologia = value;
                OnPropertyChange();
            }
        }


    }
}
