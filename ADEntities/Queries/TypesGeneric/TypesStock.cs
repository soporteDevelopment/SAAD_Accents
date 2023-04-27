using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.TypesGeneric
{
    public class TypesStock
    {

        public static short EstatusPendiente
        {
            get
            {
                return 1;
            }
        }

        public static short EstatusFinalizado
        {
            get
            {
                return 0;
            }
        }

    }
}