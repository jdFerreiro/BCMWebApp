using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMWeb.Models
{
    public class EmpresaModel : GenericModel
    {
        private long _id;

        public long Id
        {
            get { return _id; }
            set {
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

        private string _imageUrl;

        public string ImageUrl
        {
            get { return _imageUrl; }
            set {
                _imageUrl = value;
                OnPropertyChange();
            }
        }

        private long _idNivelUsuario;

        public long MyProperty
        {
            get { return _idNivelUsuario; }
            set
            {
                _idNivelUsuario = value;
                OnPropertyChange();
            }
        }



    }
}
