namespace ADEntities.ViewModels
{
    public class ActionViewModel
    {
        public int idAction { get; set; }

        public string Accion { get; set; }

        public short Ajax { get; set; }

        public short MenuPadre { get; set; }

        public short MenuHijo { get; set; }

        public string Icon { get; set; }

        public short Estatus { get; set; }

        public bool Selected { get; set; }

        public int idControl { get; set; }

        public string Control { get; set; }

        public string Descripcion { get; set; }

    }
}