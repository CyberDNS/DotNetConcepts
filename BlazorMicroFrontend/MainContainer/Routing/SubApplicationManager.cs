using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainContainer.Routing
{
    public class SubApplicationManager : ISubApplicationManager
    {
        public IEnumerable<SubApplication> SubApplications { get; }

        public SubApplicationManager(IOptionsMonitor<SubApplicationManagerOptions> options)
        {
            SubApplications = options.CurrentValue.SubApplications.ToArray();
        }

    }
}
