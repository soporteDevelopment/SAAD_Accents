using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.ViewModels
{
    /// <summary>
    /// CatalogViewViewModel
    /// </summary>
    /// <seealso cref="ADEntities.ViewModels.BaseViewModel" />
    public class CatalogViewViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the identifier vista.
        /// </summary>
        /// <value>
        /// The identifier vista.
        /// </value>
        public int idVista { get; set; }
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        public string Numero { get; set; }
        /// <summary>
        /// Gets or sets the identifier usuario.
        /// </summary>
        /// <value>
        /// The identifier usuario.
        /// </value>
        public int idUsuario { get; set; }
        /// <summary>
        /// Gets or sets the usuario.
        /// </summary>
        /// <value>
        /// The usuario.
        /// </value>
        public string Usuario { get; set; }
        /// <summary>
        /// Gets or sets the cliente.
        /// </summary>
        /// <value>
        /// The cliente.
        /// </value>
        public string NombreCliente { get; set; }
        /// <summary>
        /// Gets or sets the identifier cliente fisico.
        /// </summary>
        /// <value>
        /// The identifier cliente fisico.
        /// </value>
        public Nullable<int> idClienteFisico { get; set; }
        /// <summary>
        /// Gets or sets the identifier cliente moral.
        /// </summary>
        /// <value>
        /// The identifier cliente moral.
        /// </value>
        public Nullable<int> idClienteMoral { get; set; }
        /// <summary>
        /// Gets or sets the identifier despacho.
        /// </summary>
        /// <value>
        /// The identifier despacho.
        /// </value>
        public Nullable<int> idDespacho { get; set; }
        /// <summary>
        /// Gets or sets the identifier despacho referencia.
        /// </summary>
        /// <value>
        /// The identifier despacho referencia.
        /// </value>
        public Nullable<int> idDespachoReferencia { get; set; }
        /// <summary>
        /// Gets or sets the identifier sucursal.
        /// </summary>
        /// <value>
        /// The identifier sucursal.
        /// </value>
        public int idSucursal { get; set; }
        /// <summary>
        /// Gets or sets the sucursal.
        /// </summary>
        /// <value>
        /// The sucursal.
        /// </value>
        public string Sucursal { get; set; }
        /// <summary>
        /// Gets or sets the fecha.
        /// </summary>
        /// <value>
        /// The fecha.
        /// </value>
        public DateTime? Fecha { get; set; }
        /// <summary>
        /// Gets or sets the cantidad productos.
        /// </summary>
        /// <value>
        /// The cantidad productos.
        /// </value>
        public decimal CantidadProductos { get; set; }
        /// <summary>
        /// Gets or sets the cantidad productos.
        /// </summary>
        /// <value>
        /// The cantidad productos.
        /// </value>
        public decimal CantidadPendiente { get; set; }
        /// <summary>
        /// Gets or sets the subtotal.
        /// </summary>
        /// <value>
        /// The subtotal.
        /// </value>
        public decimal Subtotal { get; set; }
        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        /// <value>
        /// The total.
        /// </value>
        public decimal Total { get; set; }
        /// <summary>
        /// Gets or sets the estatus.
        /// </summary>
        /// <value>
        /// The estatus.
        /// </value>
        public int Estatus { get; set; }
        /// <summary>
        /// Gets or sets the color estatus.
        /// </summary>
        /// <value>
        /// The color estatus.
        /// </value>
        public string ColorEstatus { get; set; }
        /// <summary>
        /// Gets or sets the tipo cliente.
        /// </summary>
        /// <value>
        /// The tipo cliente.
        /// </value>
        public byte TipoCliente { get; set; }
        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        /// <value>
        /// The detail.
        /// </value>
        public List<CatalogDetailViewViewModel> Detalle { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public UserViewModel Vendedor { get; set; }
        /// <summary>
        /// Gets or sets the cliente.
        /// </summary>
        /// <value>
        /// The cliente.
        /// </value>
        public CustomerViewModel Cliente { get; set; }       
    }
}