using System.Collections.Generic;

namespace ADEntities.ViewModels
{
    public class ActionsControllerViewModel
    {

        public int idControl { get; set; }

        public string Control { get; set; }

        public List<ActionViewModel> lAcciones { get; set; }

    }
}