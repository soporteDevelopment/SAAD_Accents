using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.TypesGeneric
{
    public class TypesGiftsTable
    {

        public static short Todos { get { return 0; } }

        public static short Saldada { get { return 1; } }

        public static short Pendiente { get { return 2; } }

        public static short Cancelada { get { return 3; } }

        public static short PendienteVenta { get { return 1; } }

        public static short SaldadoVenta { get { return 2; } }

        public static short producto { get { return 1; } }

        public static short servicio { get { return 2; } }

        public static short credito { get { return 3; } }

    }
}