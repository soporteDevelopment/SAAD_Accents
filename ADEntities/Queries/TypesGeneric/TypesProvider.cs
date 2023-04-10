using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADEntities.Queries.TypesGeneric
{
    public class TypesProvider
    {
        public static short EstatusActivo
        {
            get
            {
                return 1;
            }
        }

        public static short EstatusInactivo
        {
            get
            {
                return 0;
            }
        }
    }
}
