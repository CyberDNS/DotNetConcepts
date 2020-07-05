using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainContainer.Routing
{
    public static class SubApplicationExtensions
    {
        public static IServiceCollection AddSubApplications(this IServiceCollection services, Action<SubApplicationManagerOptions> configureAction)
        {
            services.Configure(configureAction);
            services.AddTransient<ISubApplicationManager, SubApplicationManager>();
            return services;
        }

    }
}
