using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.TypesGeneric
{
    public class TypesReceptions
    {
        public static short CompletedReception { get { return 1; } }

        public static short PendingReception { get { return 2; } }

        public static short CanceledReception { get { return 3; } }

        public static string TxtCompletedReception { get { return "Completada"; } }

        public static string TxtPendingReception { get { return "Pendiente"; } }

        public static string TxtCanceledReception { get { return "Cancelada"; } }
    }
}