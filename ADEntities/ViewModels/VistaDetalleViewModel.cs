using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.ViewModels
{
    public class VistaDetalleViewModel
    {
        public int idDetalleVista { get; set;}
        public int idVista { get; set; }               
        public int idProducto { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }        
        public int idSucursal { get; set; }        
    }
}
