using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wombit.Plugin.Widgets.BetterDocs;

namespace Wombit.Plugin.Widgets.BetterDocs.Infrastructure
{
    /// <summary>
    /// Represents plugin route provider
    /// </summary>
    public class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute("BetterDocs",
                    "Areas/Admin/Controllers",
                    new { controller = "AdminBetterDocsController", action = "Index" });

        }
        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => 2000;
    }
}
