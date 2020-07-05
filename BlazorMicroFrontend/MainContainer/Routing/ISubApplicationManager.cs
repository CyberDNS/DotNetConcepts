using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainContainer.Routing
{
    public interface ISubApplicationManager
    {
        IEnumerable<SubApplication> SubApplications { get; }
    }
}
