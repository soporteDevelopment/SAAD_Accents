using ADEntities.Models;
using System;
using System.Collections.Generic;

namespace ADEntities.ViewModels
{
    public class OutUnifyProductsModel {
        public byte? TipoCliente { get; set; }
        public int? Customer { get; set; }
        public int? idDespachoReferencia { get; set; }
        //public string Despacho { get; set; }
        public string Proyecto { get; set; }        
        public string Vendedor { get; set; }
        public string Vendedor2 { get; set; }
        public int? idUsuario { get; set; }
        public List<OutProductsViewModel> oDetail { get; set; }
    }

    public class OutProductsViewModel
    {
        public int idVista { get; set; }
        public string remision { get; set; }
        public int? idUsuario1 { get; set; }
        public int? idUsuario2 { get; set; }
        public int? idClienteFisico { get; set; }
        public tClientesFisico oClienteFisico { get; set; }
        public int? idClienteMoral { get; set; }
        public tClientesMorale oClienteMoral { get; set; }
        public string ClienteFisico { get; set; }
        public string ClienteMoral { get; set; }
        public int? idDespacho { get; set; }
        public string Despacho { get; set; }
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
        public List<OutProductsDetailViewModel> oDetail { get; set; }
        public string Vendedor { get; set; }
        public string Vendedor2 { get; set; }
        public string Sucursal { get; set; }
        public string sEstatus { get; set; }
        public string sCustomer { get; set; }
        public bool bDevolucion { get; set; }
        public bool bVenta { get; set; }        
        public int? idVerificador { get; set; }
        public string Verificador { get; set; }
        public AddressViewModel oAddress { get; set; }
        public List<ServiceOutProductViewModel> oServicios {get; set;}
        public string RemisionVenta { get; set; }
        public bool? Flete { get; set; }
        public string Proyecto { get; set; }
        public decimal? SumSubtotal { get; set; }
        public bool Selected { get; set; }
    }

    public class OutProductsDetailViewModel 
    {
        public int idDetalleVista { get; set; }
        public int idVista { get; set; }
        public int? idProducto { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? Pendiente { get; set; }
        public decimal? Devolucion { get; set; }
        public decimal? Venta { get; set; }
        public string Sucursal { get; set; }
        public short? iEstatus { get; set; }
        public string Estatus { get; set; }
        public int? TipoImagen { get; set; }
        public string urlImagen { get; set; }
        public string RemisionVenta { get; set; }
        public int? idUsuario { get; set; }
        public string Usuario { get; set; }
        public string Extension { get; set; }
        public string IdGUID { get; set; }
        public string Comentarios { get; set; }
        public List<DetailDevOutProductViewModel> oHistoryDetail { get; set; }
        public string Remision { get; set; }
        public int numDevolucion { get; set; }
        public int numVenta { get; set; }
        public string Imagen { get; internal set; }
    }

    public class VistaViewModel
    {
        public int Id { get; set; }
        public string Remision { get; set; }
        public string ClienteFisico { get; set; }
        public string ClienteMoral { get; set; }
        public string Despacho { get; set; }
        public DateTime? Fecha { get; set; }
    }

    public class VistaDetalleViewModel
    {
        public int Id { get; set; }
        public decimal? Cantidad { get; set; }
        public string Nombre { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public string Comentarios { get; set; }
    }

}
