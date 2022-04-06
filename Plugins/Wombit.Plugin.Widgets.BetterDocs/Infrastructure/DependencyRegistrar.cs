using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure.DependencyManagement;
using Wombit.Plugin.Widgets.BetterDocs.Services;
using Wombit.Plugin.Widgets.BetterDocs.Factories;


namespace Wombit.Plugin.Widgets.BetterDocs.Infrastructure
{

    public class DependencyRegistrar : IDependencyRegistrar
    {

        public virtual void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IDocumentModelFactory, DocumentModelFactory>();
        }

        public int Order => 99;
    }
}