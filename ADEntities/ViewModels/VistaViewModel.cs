using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.ViewModels
{
    public class VistaViewModel
    {
        public int idVista { get; set;}
        public string Remision { get; set; }
        public int idUsuario1 { get; set; }
        public int idClienteFisico { get; set; }
        public int idClienteMoral { get; set; }
        public int idDespacho { get; set; }
        public int idSucursal { get; set; }
        public DateTime Fecha { get; set; }
        public int CantidadProductos { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public decimal Estatus { get; set; }

    }
}
