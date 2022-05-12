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
    public class BetterDocsPlugin : BasePlugin, IAdminMenuPlugin, IWidgetPlugin
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

        public bool HideInWidgetList => false;

        public string GetWidgetViewComponentName(string widgetZone)
        {
               
                return "WidgetsBetterDocs";
        }

        public async Task<IList<string>> GetWidgetZonesAsync()
        {
            return await Task.FromResult(new List<string> {
                AdminWidgetZones.ProductDetailsBlock,
                "ProductDetailsAfterProductSpecification"
            });
        }

        public Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "Admin.BetterDocs",
                Title = "BetterDocs",
                ControllerName = "BetterDocs",
                ActionName = "Configure",
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
        public override async Task InstallAsync()
        {

            await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Widgets.BetterDocs.Admin.Fields.FileName"] = "File Name",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.Title"] = "Title",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.DownloadId"] = "Upload File",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.Title"] = "Title",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.Documents"] = "Documents",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.Documents.Info"] = "Document Info",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.Documents.Products"] = "Products",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.FileName.Input"] = "File Name: ",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.DownloadId.Input"] = "Upload File: ",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.Title.Input"] = "Title: ",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.ProductName"] = "Product Name",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.SearchProductName"] = "Search product name:",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.SearchProductType"] = "Search product type",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.Documents.AddNew"] = "Add new document",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.BackToList"] = "Back to list",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.Documents.Edit"] = "Edit document details",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.Products.AddNew"] = "Add new product",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.BetterDocs"] = "BetterDocs",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.SaveBeforeEdit"] = "Please save document before adding products",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.Upload"] = "Upload file",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.ReplaceUpload"] = "Replace file",
                ["Plugins.Widgets.BetterDocs.Admin.Fields.Documents.Edit"] = "Edit Document"

            });

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.BetterDocs");

            await base.UninstallAsync();
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
