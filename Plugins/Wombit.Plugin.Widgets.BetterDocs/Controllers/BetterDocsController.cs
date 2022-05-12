using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Catalog;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wombit.Plugin.Widgets.BetterDocs.Domain;
using Wombit.Plugin.Widgets.BetterDocs.Factories;
using Wombit.Plugin.Widgets.BetterDocs.Models;
using Wombit.Plugin.Widgets.BetterDocs.Services;

namespace Wombit.Plugin.Widgets.BetterDocs.Controllers
{
    [Area(AreaNames.Admin)]
    [ValidateIpAddress]
    [AuthorizeAdmin]
    public class BetterDocsController : BasePluginController
    {


        private readonly IDocumentService _documentService;
        private readonly IDocumentModelFactory _documentModelFactory;
        private readonly IProductService _productService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly IDownloadService _downloadService;

        public BetterDocsController(
           IDocumentService documentService,
           IDocumentModelFactory documentModelFactory,
           IProductService productService,
           IPermissionService permissionService,
           IWorkContext workContext,
           IDownloadService downloadService)
        {
            _documentService = documentService;
            _documentModelFactory = documentModelFactory;
            _productService = productService;
            _permissionService = permissionService;
            _workContext = workContext;
            _downloadService = downloadService;
        }
        public async Task<IActionResult> Configure()
        {

            //var documents = await _documentService.GetDocumentsAsync();

            //var models = new List<DocumentModel>();

            //foreach (var document in documents)
            //{
            //    models.Add(new DocumentModel
            //    {
            //        Title = document.Title
            //    });
            //}

            var model = await _documentModelFactory.PrepareDocumentSearchModelAsync(new DocumentSearchModel());

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
            var model = await _documentModelFactory.PrepareDocumentModelAsync(new DocumentModel(), null);

            return View("~/Plugins/Widgets.BetterDocs/Views/Create.cshtml", model);
        }

        public virtual async Task<IActionResult> Edit(int id)
        {


            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return RedirectToAction("Configure");

            var model = await _documentModelFactory.PrepareDocumentModelAsync(null, document);

            return View("~/Plugins/Widgets.BetterDocs/Views/Edit.cshtml", model);
        }

        //[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        //public virtual async Task<IActionResult> Edit(DocumentModel model, bool continueEditing)
        //{

        //    var document = await _documentService.GetDocumentByIdAsync(model.Id);
        //    if (document == null)
        //        return RedirectToAction("Configure");

        //    if (ModelState.IsValid)
        //    {

        //        //document = model.ToEntity(document);
        //        //await _documentService.UpdateAsync(document);


        //        await _documentService.UpdateDocumentInfoAsync(document.Id,
        //        await _documentService.LoadDocumentBinaryAsync(document),
        //            document.ContentType,
        //            document.SeoFilename,
        //            model.Title);

        //        if (!continueEditing)
        //            return RedirectToAction("Configure");

        //        return RedirectToAction("Edit", new { id = document.Id });
        //    }

        //    model = await _documentModelFactory.PrepareDocumentModelAsync(model, document);

        //    return View("~/Plugins/Widgets.BetterDocs/Views/Edit.cshtml", model);
        //}

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {

            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return RedirectToAction("Configure");

            await _documentService.DeleteDocumentAsync(document);



            return RedirectToAction("Configure");
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

        [HttpPost]
        public virtual async Task<IActionResult> ProductList(DocumentMappingSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageCategories))
                return await AccessDeniedDataTablesJson();

            var document = await _documentService.GetDocumentByIdAsync(searchModel.DocumentId)
                ?? throw new ArgumentException("No document found with the specified id");

            var model = await _documentModelFactory.PrepareDocumentMappingListModelAsync(searchModel, document);

            return Json(model);
        }

        public virtual async Task<IActionResult> ProductUpdate(DocumentMappingModel model)
        {

            var documentMapping = await _documentService.GetDocumentMappingByIdAsync(model.Id)
                ?? throw new ArgumentException("No product document mapping found with the specified id");

            documentMapping = model.ToEntity(documentMapping);
            await _documentService.UpdateDocumentMappingAsync(documentMapping);

            return new NullJsonResult();
        }

        public virtual async Task<IActionResult> ProductDelete(int id)
        {

            var documentMapping = await _documentService.GetDocumentMappingByIdAsync(id)
                ?? throw new ArgumentException("No product document mapping found with the specified id", nameof(id));

            await _documentService.DeleteDocumentMappingAsync(documentMapping);

            return new NullJsonResult();
        }

        public virtual async Task<IActionResult> ProductAddPopup(int documentId)
        {

            var model = await _documentModelFactory.PrepareAddProductToDocumentSearchModelAsync(new AddMappingToDocumentSearchModel());

            return View("~/Plugins/Widgets.BetterDocs/Views/ProductAddPopup.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ProductAddPopupList(AddMappingToDocumentSearchModel searchModel)
        {

            var model = await _documentModelFactory.PrepareAddProductToDocumentListModelAsync(searchModel);

            return Json(model);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public virtual async Task<IActionResult> ProductAddPopup(AddMappingToDocumentModel model)
        {

            var selectedProducts = await _productService.GetProductsByIdsAsync(model.SelectedProductIds.ToArray());
            if (selectedProducts.Any())
            {
                var existingDocumentMappings = await _documentService.GetDocumentMappingsByDocumentIdAsync(model.DocumentId, showHidden: true);
                foreach (var product in selectedProducts)
                {

                    if (_documentService.FindDocumentMapping(existingDocumentMappings, product.Id, model.DocumentId) != null)
                        continue;


                    await _documentService.InsertDocumentMappingAsync(new DocumentMapping
                    {
                        DocumentId = model.DocumentId,
                        EntityId = product.Id,
                        KeyGroup = "Product",
                        DisplayOrder = 1
                    });
                }
            }

            ViewBag.RefreshPage = true;

            return View("~/Plugins/Widgets.BetterDocs/Views/ProductAddPopup.cshtml", new AddMappingToDocumentSearchModel());
        }

        [HttpPost]
        //do not validate request token (XSRF)
        [IgnoreAntiforgeryToken]
        public virtual async Task<IActionResult> AsyncUpload()
        {
            var httpPostedFile = Request.Form.Files.FirstOrDefault();
            if (httpPostedFile == null)
            {
                return Json(new
                {
                    success = false,
                    message = "No file uploaded"
                });
            }
            const string qqFileNameParameter = "qqfilename";

            var existingId = Request.Form.ContainsKey("id")
                ? Request.Form["id"].ToString()
                : string.Empty;

            var shouldContinue = Request.Form.ContainsKey("shouldContinue")
               ? Request.Form["shouldContinue"].ToString()
               : string.Empty;

            var title = Request.Form.ContainsKey("title")
                ? Request.Form["title"].ToString()
                : string.Empty;

            var qqFileName = Request.Form.ContainsKey(qqFileNameParameter)
                ? Request.Form[qqFileNameParameter].ToString()
                : string.Empty;


            var document = new Document();

            var uploadedOnUTC = DateTime.UtcNow;
            var uploadedBy = _workContext.GetCurrentCustomerAsync().Result.Username;
            var displayOrder = 1;


            if (existingId != "0")
            {
                document.Id = Int32.Parse(existingId);

                await _documentService.UpdateDocumentAsync(document.Id, httpPostedFile, title, uploadedOnUTC, uploadedBy, displayOrder, qqFileName);
            }
            else
            {
                document = await _documentService.InsertDocumentAsync(httpPostedFile, title, uploadedOnUTC, uploadedBy, displayOrder, qqFileName);
            }


            if (document == null)
                return Json(new { success = false, message = "Wrong file format" });


            if(shouldContinue == "false")
            {
                return Json(new
                {
                    success = true,
                    documentId = document.Id,
                    //documentUrl = (await _documentService.GetDocumentUrlAsync(document)).Url'
                    documentUrl = Url.Action("DownloadFile", new { id = document.Id }),
                    Url = "Configure"
                });
            }
            else
            {
                return Json(new
                {
                    success = true,
                    documentId = document.Id,
                    //documentUrl = (await _documentService.GetDocumentUrlAsync(document)).Url'
                    documentUrl = Url.Action("DownloadFile", new { id = document.Id }),
                    Url = "Edit"
                });
            }
        }


        public virtual async Task<IActionResult> DownloadFile(int id)
        {

            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return Content("No download record found with the specified id");

            var documentBinary = await _documentService.LoadDocumentFromFileAsync(id, document.ContentType);

            var fileName = !string.IsNullOrWhiteSpace(document.SeoFilename) ? document.SeoFilename : document.Id.ToString();
            var contentType = !string.IsNullOrWhiteSpace(document.ContentType)
                ? document.ContentType
                : MimeTypes.ApplicationOctetStream;
            return new FileContentResult(documentBinary, contentType)
            {
                FileDownloadName = fileName + document.Extension
            };
        }

        public virtual async Task<IActionResult> AsyncUpdateInfo(DocumentModel model, bool shouldContinue)
        {
            var document = await _documentService.GetDocumentByIdAsync(model.Id);
            if (document == null)
                return RedirectToAction("Configure");

            if (ModelState.IsValid)
            {

                await _documentService.UpdateDocumentInfoAsync(document.Id,
                await _documentService.LoadDocumentBinaryAsync(document),
                    document.ContentType,
                    document.SeoFilename,
                    model.Title);

                if (!shouldContinue)
                {
                    return RedirectToAction("Configure");
                }
                else
                {
                    return RedirectToAction("Edit", new { id = document.Id });
                }
                 
            }

            model = await _documentModelFactory.PrepareDocumentModelAsync(model, document);

            return View("~/Plugins/Widgets.BetterDocs/Views/Edit.cshtml", model);

        }
    }
}
