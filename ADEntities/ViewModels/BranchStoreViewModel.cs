namespace ADEntities.ViewModels
{
    public class BranchStoreViewModel
    {

        /// <summary>
        /// Gets or sets the identifier sucursal.
        /// </summary>
        /// <value>
        /// The identifier sucursal.
        /// </value>
        public int idSucursal { get; set; }

        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        /// <value>
        /// The nombre.
        /// </value>
        public string Nombre { get; set; }

        /// <summary>
        /// Gets or sets the descripcion.
        /// </summary>
        /// <value>
        /// The descripcion.
        /// </value>
        public string Descripcion { get; set; }

        /// <summary>
        /// Gets or sets the horarios.
        /// </summary>
        /// <value>
        /// The horarios.
        /// </value>
        public string Horarios { get; set; }

        /// <summary>
        /// Gets or sets the telefono.
        /// </summary>
        /// <value>
        /// The telefono.
        /// </value>
        public string Telefono { get; set; }

        /// <summary>
        /// Gets or sets the datos fiscales.
        /// </summary>
        /// <value>
        /// The datos fiscales.
        /// </value>
        public string DatosFiscales { get; set; }

        /// <summary>
        /// Gets or sets the iva tasa.
        /// </summary>
        /// <value>
        /// The iva tasa.
        /// </value>
        public decimal IVATasa { get; set; }

    }
}