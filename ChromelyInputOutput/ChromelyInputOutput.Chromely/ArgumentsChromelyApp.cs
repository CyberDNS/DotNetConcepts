using Chromely;
using Chromely.Core;
using Chromely.Core.Configuration;
using Chromely.Core.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChromelyInputOutput.Chromely
{
    class ArgumentsChromelyApp : ChromelyBasicApp
    {

        public override void Configure(IChromelyContainer container)
        {
            base.Configure(container);
            container.RegisterSingleton(typeof(ChromelyController), Guid.NewGuid().ToString(), typeof(ArgumentsController));
        }
    }
}
