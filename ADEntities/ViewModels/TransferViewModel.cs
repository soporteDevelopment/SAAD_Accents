using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class TransferViewModel
    {
        public int idTransferencia { get; set; }
        public string Numero { get; set; }
        public Nullable<int> idUsuario { get; set; }
        public string Usuario { get; set; }
        public Nullable<int> idSucursalOrigen { get; set; }
        public string SucursalOrigen { get; set; }
        public Nullable<int> idSucursalDestino { get; set; }
        public string SucursalDestino { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<decimal> CantidadProductos { get; set; }
        public Nullable<decimal> CostoTotal { get; set; }
        public Nullable<decimal> Estatus { get; set; }
        public string sEstatus { get; set; }
        public string ColorEstatus { get; set; }
        public string Comentarios { get; set; }
        public Nullable<int> CreadoPor { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
        public Nullable<int> ModificadoPor { get; set; }
        public Nullable<System.DateTime> Modificado { get; set; }
        public bool Update { get; set; } = false;
        public List<DetailTransferViewModel> lDetail { get; set; }
    }
}