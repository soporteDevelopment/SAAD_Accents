using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADEntities.Queries.TypesGeneric
{
    public class TypesCatalogProvider
    {
        public static bool Active
        {
            get
            {
                return true;
            }
        }

        public static bool Deactive
        {
            get
            {
                return false;
            }
        }
    }
}