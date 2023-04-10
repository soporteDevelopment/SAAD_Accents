using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.TypesGeneric
{
    public static class TypesIntelligence
    {
        public static int Comision {
            get
            {
                return 1;
            }
        }

        public static int Bono
        {
            get
            {
                return 2;
            }
        }

        public static short Active
        {
            get
            {
                return 1;
            }
        }

        public static short Cancelled
        {
            get
            {
                return 2;
            }
        }
    }
}