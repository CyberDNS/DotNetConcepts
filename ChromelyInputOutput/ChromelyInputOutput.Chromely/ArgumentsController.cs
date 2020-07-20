using Chromely.Core.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChromelyInputOutput.Chromely
{
    public class ArgumentsController : ChromelyController
    {

        public ArgumentsController()
        {
            RegisterGetRequest("/arguments/get", GetArguments);
        }

        private ChromelyResponse GetArguments(ChromelyRequest request)
        {
            return new ChromelyResponse() { Data = "Hello World Arguments" };
        }

    }
}
