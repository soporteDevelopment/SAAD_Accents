using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.Queries.TypesGeneric
{
    public static class TypesCredit
    {
        public static short creditoTodos { get { return 0; } }
        public static short creditoSaldada { get { return 1; } }
        public static short creditoPendiente { get { return 2; } }
        public static short creditoCancelada { get { return 3; } }
    }
}
