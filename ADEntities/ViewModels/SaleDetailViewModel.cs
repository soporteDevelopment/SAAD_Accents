namespace ADEntities.ViewModels
{
    public class SaleDetailViewModel
    {

        public int idDetalleVenta { get; set; }

        public int idVenta { get; set; }

        public int? idProducto { get; set; }

        public ProductViewModel oProducto { get; set; }

        public int? idServicio { get; set; }

        public int? idNotaCredito { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public decimal? Precio { get; set; }

        public decimal? Descuento { get; set; }

        public decimal? Cantidad { get; set; }

        public bool? NotaCredito { get; internal set; }

        public short? TipoImagen { get; internal set; }

        public string Imagen { get; set; }

        public short? Tipo { get; set; }

        public string IdGUID { get; set; }

        public string Comentarios { get; set; }

        public decimal? Porcentaje { get; set; } = 0;

        public bool? Omitir { get; set; }

        public int? idSucursal { get; set; }

        public string Sucursal { get; set; }

        public int? idVista { get; set; }

        public string Remision { get; set; }

        public string Proveedor { get; set; }

        public int? idPromocion { get; set; }

        public decimal? CostoPromocion { get; set; }

        public int? idTipoPromocion { get; set; }

        public int? idProductoPadre { get; set; }
    }
}
