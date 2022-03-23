using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Media;
using Nop.Services.Media;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework.Controllers;
using Wombit.Plugin.Widgets.BetterDocs.Services;
using Wombit.Plugin.Widgets.BetterDocs.Areas.Admin.Models;

namespace Wombit.Plugin.Widgets.BetterDocs.Areas.Admin.Controllers
{
    [Area(AreaNames.Admin)]
    [ValidateIpAddress]
    [AuthorizeAdmin]
    public class AdminBetterDocsController : BasePluginController
    {


        private readonly IDocumentService _documentService;

        public AdminBetterDocsController(
           IDocumentService documentService)
        {
            _documentService = documentService;
        }
        public async Task<IActionResult> Index()
        {

           var documents = await _documentService.GetDocumentsAsync();

            var models = new List<DocumentListModel>();

            foreach (var document in documents)
            {
                models.Add(new DocumentListModel
                {
                    Title = document.Title
                });
            }

            //foreach (var setting in resultSettings)
            //{
            //    var model = new PublicInfoModel()
            //    {
            //        DisplayText = setting.Value
            //    };

            //    models.Add(model);
            //}

            return View("~/Plugins/Widgets.BetterDocs/Areas/Admin//Views/Default.cshtml", models);

        }

    }
}
