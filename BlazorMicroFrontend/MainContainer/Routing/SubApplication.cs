using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainContainer.Routing
{
    public class SubApplication
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public Uri Uri { get; set; }

        public string Route => $"/app/{Key}";
    }
}
