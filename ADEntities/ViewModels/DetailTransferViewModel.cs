﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    public class DetailTransferViewModel
    {
        public int idDetalleTransferencia { get; set; }
        public Nullable<int> idTransferencia { get; set; }
        public Nullable<int> idProducto { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public Nullable<decimal> Cantidad { get; set; }
        public Nullable<decimal> CantidadEnviada { get; set; }
        public Nullable<decimal> Costo { get; set; }
        public Nullable<int> Estatus { get; set; }
        public string Comentarios { get; set; }
        public Nullable<int> CreadoPor { get; set; }
        public Nullable<System.DateTime> Creado { get; set; }
        public Nullable<int> ModificadoPor { get; set; }
        public Nullable<System.DateTime> Modificado { get; set; }
    }
}