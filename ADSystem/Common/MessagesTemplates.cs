using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADSystem.Common
{
    public class MessagesTemplates 
    {
        private string message;

        public String msgAdd
        {

            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }

    }
}