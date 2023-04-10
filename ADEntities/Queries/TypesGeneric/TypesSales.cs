using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.Queries.TypesGeneric
{
    public static class TypesSales
    {
        public static short ventaTodos { get { return 0; } }

        public static short ventaSaldada { get { return 1; } }

        public static short ventaPendiente { get { return 2; } }

        public static short ventaCancelada { get { return 3; } }

        public static short serviceTrue { get { return 1; } }

        public static short serviceFalse { get { return 0; } }

        public static short factura { get { return 1; } }

        public static short producto { get { return 1; } }

        public static short servicio { get { return 2; } }

        public static short credito { get { return 3; } }

        public static short vista { get { return 2; } }

        public static short cotizacion { get { return 3; } }

        public static short normal { get { return 1; } }

    }
}
