using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADEntities.Models;

namespace ADEntities.ViewModels
{
    public class DetailGiftsTableViewModel
    {

        public int? idDetalle { get; set; }

        public int? idMesaRegalo { get; set; }

        public int? idProducto { get; set; }

        public string Descripcion { get; set; }

        public ProductViewModel Producto { get; set; }

        public int? idServicio { get; set; }

        public decimal? Precio { get; set; }

        public decimal? Cantidad { get; set; }

        public decimal? Porcentaje { get; set; }

        public decimal? Descuento { get; set; }

        public short? Tipo { get; set; }

        public string Comentario { get; set; }

        public short? Estatus { get; set; }

        public string sEstatus { get; set; }

        public string IdGUID { get; set; }

    }
}