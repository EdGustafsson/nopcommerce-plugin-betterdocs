using System.Collections.Generic;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;
using System.Linq;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework;

namespace Wombit.Plugin.Widgets.BetterDocs
{
    public class BetterDocsPlugin : BasePlugin, IAdminMenuPlugin
    {
      

        private readonly ILocalizationService _localizationService;
        private readonly WidgetSettings _widgetSettings;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

     

        public BetterDocsPlugin(
            ILocalizationService localizationService,
            WidgetSettings widgetSettings,
            ISettingService settingService,
            IWebHelper webHelper)
        {
            _localizationService = localizationService;
            _widgetSettings = widgetSettings;
            _settingService = settingService;
            _webHelper = webHelper;
        }

        public Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "Admin.BetterDocs",
                Title = "BetterDocs",
                ControllerName = "AdminBetterDocs",
                ActionName = "Index",
                IconClass = "far fa-dot-circle",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.Admin } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);

            return Task.CompletedTask;
        }


        //public bool HideInWidgetList => false;

        //public string GetWidgetViewComponentName(string widgetZone)
        //{
        //    if (widgetZone == AdminWidgetZones.ProductDetailsBlock)
        //    {
        //        return "ProductDetailsAdmin";
        //    }
        //    else if (widgetZone == "ProductDetailsAfterProductSpecification")
        //    {
        //        return "ProductDownloadsComponent";
        //    }

        //    return "Documents";
        //}

        //public  Task<IList<string>> GetWidgetZonesAsync()
        //{
        //    return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HeaderAfter });
        //}

        //public override async Task InstallAsync()
        //{
        //    await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
        //    {
        //        ["Plugins.Widgets.BetterDocs.Title"] = "Documents",
        //        ["Plugins.Widgets.BetterDocs.Fields.DownloadId"] = "File",
        //        ["Plugins.Widgets.BetterDocs.Fields.DisplayOrder"] = "DisplayOrder",
        //        ["Plugins.Widgets.BetterDocs.Fields.Title"] = "Title",
        //        ["Plugins.Widgets.BetterDocs.Required"] = "{0} is required",
        //        ["Plugins.Widgets.BetterDocs.SaveBeforeEdit"] = "You need to save before adding documents"
        //    });

        //    await base.InstallAsync();
        //}

        //public override async Task UninstallAsync()
        //{
        //    await _localizationService.DeleteLocaleResourceAsync("Plugins.Widgets.BetterDocs");

        //    await base.UninstallAsync();
        //}


    }
}
