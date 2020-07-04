using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmbeddedApp1
{
    public class NavigationMessage
    {
        public string Message => "navigateMain";

        public string Application { get; set; }
        public string Url { get; set; }
    }
}
