using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;
using System.Collections.Generic;
using Wombit.Plugin.Widgets.BetterDocs.Models;
using System.Linq;
using Wombit.Plugin.Widgets.BetterDocs.Services;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Tax;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Areas.Admin.Models.Customers;
using Nop.Web.Areas.Admin.Models.Orders;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Models;
using Wombit.Plugin.Widgets.BetterDocs.Domain;
using Nop.Core.Domain.Media;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Services.Messages;
using Nop.Web.Framework.Controllers;
using Wombit.Plugin.Widgets.BetterDocs.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Microsoft.AspNetCore.Http;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Discounts;
using Nop.Services.Discounts;
using Nop.Services.ExportImport;
using Nop.Services.Logging;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Wombit.Plugin.Widgets.BetterDocs.Components
{
    

    [ViewComponent(Name = "WidgetsBetterDocs")]
    public class WidgetsBetterDocsViewComponent : NopViewComponent
    {

        private readonly IDocumentService _documentService;
        private readonly IProductService _productService;
        private readonly IDocumentModelFactory _documentModelFactory;
        public WidgetsBetterDocsViewComponent(IDocumentService documentService,
            IProductService productService,
            IDocumentModelFactory documentModelFactory)
        {
            _documentService = documentService;
            _productService = productService;
            _documentModelFactory = documentModelFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {

            if (additionalData is not BaseNopEntityModel entityModel)
                return Content(string.Empty);

            var entity = await _productService.GetProductByIdAsync(entityModel.Id);

            if (widgetZone.Equals(AdminWidgetZones.ProductDetailsBlock))
            {

                var documentList = await _documentService.GetDocumentsByMappingEntityId(entity.Id, "Product");

                var documentDisplayModelList = await _documentModelFactory.PreparePublicInfoModelAsync(documentList.ToList());

                var model = JsonConvert.SerializeObject(documentDisplayModelList, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                return View("~/Plugins/Widgets.BetterDocs/Views/PublicInfo.cshtml", model);
            }

            return View("");

        }
    }
}
