using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADTASKS.Models
{
    class ViewModel
    {
        public int idVista { get; set; }
        public string Remision { get; set; }
        public int? idUsuario1 { get; set; }
        public int? idUsuario2 { get; set; }
        public string Vendedor { get; set; }
        public int? idClienteFisico { get; set; }
        public int? idClienteMoral { get; set; }
        public string Cliente { get; set; }
        public int? idDespacho { get; set; }
        public int? idDespachoReferencia { get; set; }
        public string DespachoReferencia { get; set; }
        public byte? TipoCliente { get; set; }
        public int? idSucursal { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal? CantidadProductos { get; set; }
        public decimal? ProductosRestantes { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Total { get; set; }
        public decimal? Restante { get; set; }
        public short? iEstatus { get; set; }
        public string Estatus { get; set; }
        public string Proyecto { get; set; }
    }
}
