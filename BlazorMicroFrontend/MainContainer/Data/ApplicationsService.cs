using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainContainer.Data
{
    public class ApplicationsService
    {
        public IEnumerable<Application> Applications { get; set; } = new Application[] 
        { 
            new Application { Name = "Embedded App 1", Url = new Uri("http://localhost:5001") },
            new Application { Name = "Embedded App 2", Url = new Uri("http://localhost:5002") }
        };
    }
}
