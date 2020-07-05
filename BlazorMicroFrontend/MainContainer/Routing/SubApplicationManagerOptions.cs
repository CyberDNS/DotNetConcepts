using MainContainer.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainContainer.Routing
{
    public class SubApplicationManagerOptions
    {
        public List<SubApplication> SubApplications = new List<SubApplication>();
    }
}
