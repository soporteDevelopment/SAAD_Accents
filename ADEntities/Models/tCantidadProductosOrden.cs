//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ADEntities.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tCantidadProductosOrden
    {
        public int idtCantidadProductosOrden { get; set; }
        public int idOrden { get; set; }
        public int idProducto { get; set; }
        public int idSucursal { get; set; }
        public Nullable<int> Cantidad { get; set; }
    
        public virtual tOrden tOrden { get; set; }
        public virtual tProducto tProducto { get; set; }
        public virtual tSucursale tSucursale { get; set; }
    }
}
