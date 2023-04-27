using System;

namespace ADEntities.ViewModels
{
    public class OrderViewModel
    {

        public int idOrden { get; set; }

        public string Orden { get; set; }

        public string Factura { get; set; }

        public int idEmpresa { get; set; }

        public string Empresa { get; set; }

        public Nullable<DateTime> FechaCompra { get; set; }

        public DateTime FechaEntrega { get; set; }

        public DateTime FechaCaptura { get; set; }

        public short Estatus { get; set; }


        public string sEstatus;

        public decimal? Dolar { get; set; }

        public Nullable<int> Moneda { get; set; }

    }
}