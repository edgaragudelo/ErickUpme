using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subasta
{
    class OfertasCompra
    {
        public string nombre { get; set; }
        public string ID_oferta { get; set; }
        public string bloque { get; set; }

        public int numPaquetesMax { get; set; }
        public int numPaquetesMin { get; set; }

        public double precio { get; set; }


    }
}
