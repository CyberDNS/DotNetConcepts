using Chromely;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Network;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace ChromelyInputOutput.Chromely
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = DefaultConfiguration.CreateForRuntimePlatform();
            config.WindowOptions.Title = "Chromely IO";
            config.StartUrl = "http://localhost:5000";
            config.DebuggingMode = true;
            //config.ControllerAssemblies = new List<ControllerAssemblyInfo>();
            //config.ControllerAssemblies.RegisterServiceAssembly(Assembly.GetExecutingAssembly());

            AppBuilder
                .Create()
                .UseConfiguration<DefaultConfiguration>(config)
                .UseApp<ArgumentsChromelyApp>()
                .Build()
                .Run(args);
        }
    }
}
