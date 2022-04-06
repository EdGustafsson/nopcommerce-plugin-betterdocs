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
using Wombit.Plugin.Widgets.BetterDocs.Models;
using Wombit.Plugin.Widgets.BetterDocs.Factories;
using Wombit.Plugin.Widgets.BetterDocs.Domain;

namespace Wombit.Plugin.Widgets.BetterDocs.Controllers
{
    [Area(AreaNames.Admin)]
    [ValidateIpAddress]
    [AuthorizeAdmin]
    public class BetterDocsController : BasePluginController
    {


        private readonly IDocumentService _documentService;
        private readonly IDocumentModelFactory _documentModelFactory;

        public BetterDocsController(
           IDocumentService documentService,
           IDocumentModelFactory documentModelFactory)
        {
            _documentService = documentService;
            _documentModelFactory = documentModelFactory;
        }
        public async Task<IActionResult> Configure()
        {

           var documents = await _documentService.GetDocumentsAsync();

            var models = new List<DocumentModel>();

            foreach (var document in documents)
            {
                models.Add(new DocumentModel
                {
                    Title = document.Title
                });
            }

            //prepare model
            var model = await _documentModelFactory.PrepareDocumentSearchModelAsync(new DocumentSearchModel());


            //foreach (var setting in resultSettings)
            //{
            //    var model = new PublicInfoModel()
            //    {
            //        DisplayText = setting.Value
            //    };

            //    models.Add(model);
            //}

            return View("~/Plugins/Widgets.BetterDocs/Views/Configure.cshtml", model);

        }


        [HttpPost]
        public async Task<IActionResult> List(DocumentSearchModel searchModel)
        {

            var model = await _documentModelFactory.PrepareDocumentListModelAsync(searchModel);

            return Json(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new DocumentModel();

            return View("~/Plugins/Widgets.BetterDocs/Views/Create.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DocumentModel model)
        {

            var document = new Document
            {
                DownloadId = model.DownloadId,
                Title = model.Title,
                FileName = model.FileName,
                UploadedBy = model.UploadedBy,
                UploadedOnUTC = DateTime.Now
            };

            await _documentService.InsertAsync(document);

            ViewBag.RefreshPage = true;

            return View("~/Plugins/Widgets.BetterDocs/Views/Create.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> DeleteSelected(ICollection<int> selectedIds)
        {

            if (selectedIds != null)
            {
                await _documentService.DeleteDocumentsAsync(await _documentService.GetDocumentsByIdsAsync(selectedIds.ToArray()));
            }

            return Json(new { Result = true });
        }

       
    }
}
