using System;

namespace ADEntities.ViewModels
{
    public class CreditHistoryViewModel
    {
        public int idHitorialCredito { get; set; }
        public int? idVenta { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? Restante { get; set; }
        public int idFormaPago { get; set; }
        public string FormaPago { get; set; }
        public string Comentario { get; set; }
        public DateTime? Fecha { get; set; }
        public bool bCalendar { get; set; }
        public short? Estatus { get; set; }
        public string sEstatus { get; set; }
        public decimal? amountIVA { get; set; }
        public bool? IVA { get; set; }
        public int? idCuenta { get; set; }
        public string _Cuenta { get; set; }
        public string _voucher { get; set; }
        public int? idCreditNote { get; set; }
        public string CreditNote { get; set; }
        public bool Seleccionado { get; set; }
    }
}
