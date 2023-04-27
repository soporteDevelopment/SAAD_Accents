using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ADEntities.ViewModels
{
    public class ReportOfSalesViewModel
    {       

        public int idVenta { get; set; }

        public int Anio { get; set; }

        public int Mes { get; set; }

        public int? Semana { get; set; }

        public string Dia { get; set; }

        public DateTime? Fecha { get; set; }

        public DateTime? Hora { get; set; }

        public int? idUsuario1 { get; set; }

        public string Usuario1 { get; set; }

        public int? idUsuario2 { get; set; }

        public string Usuario2 { get; set; }

        public int? idDespacho { get; set; }

        public string Despacho { get; set; }

        public int? idSucursal { get; set; }

        public string Sucursal { get; set; }

        public string Remision { get; set; }

        public string TipoCliente { get; set; }

        public int? idClienteFisico { get; set; }

        public string ClienteFisico { get; set; }

        public int? idClienteMoral { get; set; }

        public string ClienteMoral { get; set; }

        public int? idDespachoCliente { get; set; }

        public string DespachoCliente { get; set; }

        public string Cliente { get; set; }

        public decimal? ImporteVenta { get; set; }

        public short? Estatus { get; set; }

        public string sEstatus { get; set; }

        public decimal? IntDescuento { get; set; }

        public decimal? Descuento { get; set; }

        public decimal? ImportePagadoCliente { get; set; }

        public string Vendedor1 { get; set; }

        public int? IntComision1 { get; set; }

        public decimal? Comision1 { get; set; }

        public DateTime? FechaPago1 { get; set; }

        public int? FormaPago1 { get; set; }

        public string DescFormasPago1 { get; set; }

        public string Vendedor2 { get; set; }

        public int? IntComision2 { get; set; }

        public decimal? Comision2 { get; set; } 

        public DateTime? FechaPago2 { get; set; }

        public int? FormaPago2 { get; set; }

        public string DescFormasPago2 { get; set; }

        public string Despacho1 { get; set; }

        public int? IntComision3 { get; set; }

        public decimal? Comision3 { get; set; }

        public DateTime? FechaPago3 { get; set; }

        public int? FormaPago3 { get; set; }

        public string DescFormasPago3 { get; set; }

        public List<string> DescFormasPago { get; set; }

        public List<SaleDetailViewModel> oDetail { get; set; }
        
        public List<TypePaymentViewModel> listTypePayment { get; set; }
        
        public decimal? comAmericanExpress { get; set; }

        public decimal? sumImporteVenta { get; set; }

        public decimal? sumTotalDescuento { get; set; }

        public decimal? sumImportePagadoCliente { get; set; }

        public short? IVA { get; set; }

        public bool? Pagado { get; set; }

        public bool? Pagar { get; set; }

        public bool? Compartida { get; set; }

        public bool? Omitir { get; set; }

        public int? idVentasComisionesDespacho { get; set; }

        public decimal? ComisionGeneral { get; set; }

        public decimal? ComisionArterios { get; set; }

        public decimal? TotalNotaCredito { get; set; }

        public decimal? ComisionTotal { get; set; }

        public string Proyecto { get; set; }
    }

 }