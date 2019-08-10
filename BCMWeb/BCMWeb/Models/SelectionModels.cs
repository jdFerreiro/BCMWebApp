using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMWeb.Models
{
    public class IntSelectionModel : GenericModel
    {
        private int _valor;

        public int Valor
        {
            get { return _valor; }
            set
            {
                _valor = value;
                OnPropertyChange();
            }
        }

        private string _descripcion;

        public string Descripcion
        {
            get { return _descripcion; }
            set
            {
                _descripcion = value;
                OnPropertyChange();
            }
        }


    }
}
