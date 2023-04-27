using System;
using System.Collections.Generic;

namespace ADEntities.ViewModels
{
    public class ReceptionViewModel
    {
        public int idRecepcion { get; set; }       
        public string Numero { get; set; }
        public int? idTransferencia { get; set; }
        public string Transferencia { get; set; }
        public Nullable<int> idUsuario { get; set; }
        public string Usuario { get; set; }
        public Nullable<int> idSucursal { get; set; }
        public string Sucursal { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<decimal> CantidadProductos { get; set; }
        public Nullable<decimal> CostoTotal { get; set; }        
        public string Comentarios { get; set; }
        public Nullable<decimal> Estatus { get; set; }
        public string sEstatus { get; set; }
        public string ColorEstatus { get; set; }
        public Nullable<int> CreadoPor { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
        public Nullable<int> ModificadoPor { get; set; }
        public Nullable<System.DateTime> Modificado { get; set; }
        public List<DetailReceptionViewModel> lDetail { get; set; }
    }
}