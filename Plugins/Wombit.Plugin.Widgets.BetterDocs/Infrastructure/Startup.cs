using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Wombit.Plugin.Widgets.BetterDocs.Models;
using Wombit.Plugin.Widgets.BetterDocs.Validators;

namespace Wombit.Plugin.Widgets.BetterDocs.Infrastructure
{
    public class Startup : INopStartup
    {
        public int Order => 0;

        public void Configure(IApplicationBuilder application)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddTransient<IValidator<DocumentModel>, DocumentValidator>();

        }
    }
}
