using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.TypesGeneric
{
    public class TypesTransfers
    {
        public static short CompletedTransfer { get { return 1; } }

        public static short PendingTransfer { get { return 2; } }

        public static short CanceledTransfer { get { return 3; } }

        public static string TxtCompletedTransfer { get { return "Completada"; } }

        public static string TxtPendingTransfer { get { return "Pendiente"; } }

        public static string TxtCanceledTransfer { get { return "Cancelada"; } }
    }
}