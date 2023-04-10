using System;
using System.Collections.Generic;

namespace ADEntities.ViewModels
{
    public class TypePaymentViewModel
    {

        public int idTypePayment { get; set; }

        public string sTypePayment { get; set; }

        public short typesPayment { get; set; }

        public string sTypesCard { get; set; }

        public int? typesCard { get; set; }

        public string bank { get; set; }

        public string holder { get; set; }

        public string numCheck { get; set; }

        public string numIFE { get; set; }

        public decimal amount { get; set; }

        public DateTime maxPayment { get; set; }

        public string dateMaxPayment { get; set; }

        public short? Estatus { get; set; }

        public string sEstatus { get; set; }

        public DateTime? DatePayment { get; set; }

        public bool bCalendar { get; set; }

        public decimal? amountIVA { get; set; }

        public bool? IVA { get; set; }

        public AccountViewModel cuenta { get; set; }

        public int? idCuenta { get; set; }

        public string _Cuenta { get; set; }

        public string _voucher { get; set; }

        public int? idCreditNote { get; set; }

        public string CreditNote { get; set; }

        public Nullable<int> paymentMonth { get; set; }
        public bool Seleccionado { get; set; }
        public Nullable<int> idCatalogBankAccount { get; set; }
        public Nullable<int> idCatalogTerminalType { get; set; }
        public List<CreditHistoryViewModel> HistoryCredit { get; set; }
        

    }
}
