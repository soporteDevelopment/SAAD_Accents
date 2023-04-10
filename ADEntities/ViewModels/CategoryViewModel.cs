namespace ADEntities.ViewModels
{
	/// <summary>
	/// CategoryViewModel
	/// </summary>
	public class CategoryViewModel
    {
		/// <summary>
		/// Gets or sets the identifier categoria.
		/// </summary>
		/// <value>
		/// The identifier categoria.
		/// </value>
		public int idCategoria { get; set; }
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
		/// Gets or sets the imagen.
		/// </summary>
		/// <value>
		/// The imagen.
		/// </value>
		public string Imagen { get; set; }
		/// <summary>
		/// Gets or sets the subcategoria.
		/// </summary>
		/// <value>
		/// The subcategoria.
		/// </value>
		public SubcategoryViewModel Subcategoria { get; set; }
    }
}