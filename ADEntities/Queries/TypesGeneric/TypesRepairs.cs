using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.TypesGeneric
{
    public static class TypesRepairs
    {
        public static short CompletedRepair { get { return 1; } }

        public static short PendingRepair { get { return 2; } }

        public static short CanceledRepair { get { return 3; } }

        public static string TxtCompletedRepair { get { return "Completada"; } }

        public static string TxtPendingRepair { get { return "Pendiente"; } }

        public static string TxtCanceledRepair { get { return "Cancelada"; } }
    }
}