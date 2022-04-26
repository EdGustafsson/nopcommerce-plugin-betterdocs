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
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Microsoft.AspNetCore.Http;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Discounts;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.ExportImport;
using Nop.Services.Logging;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Catalog;

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
        private readonly IDocumentFileService _documentFileService;

        public BetterDocsController(
           IDocumentService documentService,
           IDocumentModelFactory documentModelFactory,
           IProductService productService,
           IPermissionService permissionService,
           IWorkContext workContext,
           IDownloadService downloadService,
           IDocumentFileService documentFileService)
        {
            _documentService = documentService;
            _documentModelFactory = documentModelFactory;
            _productService = productService;
            _permissionService = permissionService;
            _workContext = workContext;
            _downloadService = downloadService;
            _documentFileService = documentFileService;
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

        //[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        //public virtual async Task<IActionResult> Create(DocumentModel model, bool continueEditing)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var document = model.ToEntity<Document>();

        //        document.UploadedOnUTC = DateTime.UtcNow;
        //        document.UploadedBy = _workContext.GetCurrentCustomerAsync().Result.Username;
        //        document.DisplayOrder = 1;

        //        await _documentService.InsertAsync(document);

        //        await _documentService.UpdateAsync(document);

        //        if (!continueEditing)
        //            return RedirectToAction("Configure");

        //        return RedirectToAction("Edit", new { id = document.Id });
        //    }

        //    model = await _documentModelFactory.PrepareDocumentModelAsync(model, null);

        //    return View("~/Plugins/Widgets.BetterDocs/Views/Create.cshtml", model);
        //}

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Create(DocumentModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

               // var document = await _documentService.GetDocumentByIdAsync(model.Id)
               //?? throw new ArgumentException("No picture found with the specified id");

                var document = model.ToEntity<Document>();

                document.UploadedOnUTC = DateTime.UtcNow;
                document.UploadedBy = _workContext.GetCurrentCustomerAsync().Result.Username;
                document.DisplayOrder = 1;
                document.Title = model.Title;

                //await _documentService.InsertAsync(document);

                //await _documentService.UpdateAsync(document);

                await _documentFileService.UpdateDocumentAsync(document.Id,
                await _documentFileService.LoadDocumentBinaryAsync(document),
                    document.MimeType,
                    document.SeoFilename,
                    document.Title, 
                    document.UploadedOnUTC,
                    document.UploadedBy,
                    document.DisplayOrder);


                if (!continueEditing)
                    return RedirectToAction("Configure");

                return RedirectToAction("Edit", new { id = document.Id });
            }

            model = await _documentModelFactory.PrepareDocumentModelAsync(model, null);

            return View("~/Plugins/Widgets.BetterDocs/Views/Create.cshtml", model);


            //try to get a picture with the specified id
            

         

        }


        public virtual async Task<IActionResult> Edit(int id)
        {

            
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return RedirectToAction("Configure");

            var model = await _documentModelFactory.PrepareDocumentModelAsync(null, document);

            return View("~/Plugins/Widgets.BetterDocs/Views/Edit.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Edit(DocumentModel model, bool continueEditing)
        {

            var document = await _documentService.GetDocumentByIdAsync(model.Id);
            if (document == null)
                return RedirectToAction("Configure");

            if (ModelState.IsValid)
            {

                //document = model.ToEntity(document);
                //await _documentService.UpdateAsync(document);


                await _documentFileService.UpdateDocumentAsync(document.Id,
                await _documentFileService.LoadDocumentBinaryAsync(document),
                    document.MimeType,
                    document.SeoFilename,
                    model.Title);

                if (!continueEditing)
                    return RedirectToAction("Configure");

                return RedirectToAction("Edit", new { id = document.Id });
            }

            model = await _documentModelFactory.PrepareDocumentModelAsync(model, document);

            return View("~/Plugins/Widgets.BetterDocs/Views/Edit.cshtml", model);
        }

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
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task<IActionResult> AsyncUpload()
        {
            //if (!await _permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");

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

            var qqFileName = Request.Form.ContainsKey(qqFileNameParameter)
                ? Request.Form[qqFileNameParameter].ToString()
                : string.Empty;

            var document = await _documentFileService.InsertDocumentAsync(httpPostedFile, qqFileName);

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.

            if (document == null)
                return Json(new { success = false, message = "Wrong file format" });

            return Json(new
            {
                success = true,
                documentId = document.Id,
                //documentUrl = (await _documentFileService.GetDocumentUrlAsync(document)).Url'
                documentUrl = Url.Action("DownloadFile", new { id = document.Id })
            });
        }

        public virtual async Task<IActionResult> DownloadFile(int id)
        {

            // Fetch document binary from file system

            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return Content("No download record found with the specified id");

            ////use stored data
            //if (document.DownloadBinary == null)
            //    return Content($"Download data is not available any more. Download GD={download.Id}");

            var documentBinary = await _documentFileService.LoadDocumentFromFileAsync(id, document.MimeType);

            var fileName = !string.IsNullOrWhiteSpace(document.SeoFilename) ? document.SeoFilename : document.Id.ToString();
            var contentType = !string.IsNullOrWhiteSpace(document.ContentType)
                ? document.ContentType
                : MimeTypes.ApplicationOctetStream;
            return new FileContentResult(documentBinary, contentType)
            {
                FileDownloadName = fileName + document.MimeType
            };
        }

        //public virtual async Task<IActionResult> AsyncUpdate(int documentId, int displayOrder,
        //    string overrideAltAttribute, string overrideTitleAttribute, int productId)
        //{
        //    if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
        //        return AccessDeniedView();

        //    if (documentId == 0)
        //        throw new ArgumentException();

        //    //try to get a product with the specified id
        //    var product = await _productService.GetProductByIdAsync(productId)
        //        ?? throw new ArgumentException("No product found with the specified id");

        //    //a vendor should have access only to his products
        //    if (await _workContext.GetCurrentVendorAsync() != null && product.VendorId != (await _workContext.GetCurrentVendorAsync()).Id)
        //        return RedirectToAction("List");

        //    if ((await _productService.GetProductPicturesByProductIdAsync(productId)).Any(p => p.PictureId == pictureId))
        //        return Json(new { Result = false });

        //    //try to get a picture with the specified id
        //    var document = await _documentService.GetDocumentByIdAsync(documentId)
        //        ?? throw new ArgumentException("No document found with the specified id");

        //    await _documentFileService.UpdateDocumentAsync(document.Id,
        //        await _documentFileService.LoadDocumentBinaryAsync(document),
        //        document.MimeType,
        //        document.SeoFilename);

        //    //await _pictureService.SetSeoFilenameAsync(pictureId, await _pictureService.GetPictureSeNameAsync(product.Name));

        //    //await _productService.InsertProductPictureAsync(new ProductPicture
        //    //{
        //    //    PictureId = pictureId,
        //    //    ProductId = productId,
        //    //    DisplayOrder = displayOrder
        //    //});

        //    return Json(new { Result = true });
        //}



    }
}
