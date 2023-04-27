using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.IO;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(ADSystem.Startup))]

namespace ADSystem
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
