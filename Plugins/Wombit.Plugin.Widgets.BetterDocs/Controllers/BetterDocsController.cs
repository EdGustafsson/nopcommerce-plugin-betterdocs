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

        public BetterDocsController(
           IDocumentService documentService,
           IDocumentModelFactory documentModelFactory,
           IProductService productService)
        {
            _documentService = documentService;
            _documentModelFactory = documentModelFactory;
            _productService = productService;
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
            var model = await _documentModelFactory.PrepareDocumentModelAsync(new DocumentModel(), null);

            return View("~/Plugins/Widgets.BetterDocs/Views/Create.cshtml", model);
        }


        //[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        //public async Task<IActionResult> Create(DocumentModel model, bool continueEditing)
        //{

        //    var document = new Document
        //    {
        //        DownloadId = model.DownloadId,
        //        Title = model.Title,
        //        FileName = model.FileName,
        //        UploadedBy = model.UploadedBy,
        //        UploadedOnUTC = DateTime.Now
        //    };

        //    await _documentService.InsertAsync(document);

        //    ViewBag.RefreshPage = true;

        //    if (!continueEditing)
        //        return RedirectToAction("Configure");

        //    return RedirectToAction("Edit", new { id = document.Id });
        //}

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Create(DocumentModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var document = model.ToEntity<Document>();
                document.UploadedOnUTC = DateTime.UtcNow;
                await _documentService.InsertAsync(document);

                await _documentService.UpdateAsync(document);

                if (!continueEditing)
                    return RedirectToAction("~/Plugins/Widgets.BetterDocs/Views/Configure.cshtml");

                return RedirectToAction("~/Plugins/Widgets.BetterDocs/Views/Edit.cshtml", new { id = document.Id });
            }

            //prepare model
            model = await _documentModelFactory.PrepareDocumentModelAsync(model, null);

            //if we got this far, something failed, redisplay form
            return View("~/Plugins/Widgets.BetterDocs/Views/Create.cshtml", model);
        }

      
        public virtual async Task<IActionResult> Edit(int id)
        {

            
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return RedirectToAction("~/Plugins/Widgets.BetterDocs/Views/Configure.cshtml");

            var model = await _documentModelFactory.PrepareDocumentModelAsync(null, document);

            return View("~/Plugins/Widgets.BetterDocs/Views/Edit.cshtml", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Edit(DocumentModel model, bool continueEditing)
        {

            var document = await _documentService.GetDocumentByIdAsync(model.Id);
            if (document == null)
                return RedirectToAction("~/Plugins/Widgets.BetterDocs/Views/Configure.cshtml");

            if (ModelState.IsValid)
            {

                document = model.ToEntity(document);
                await _documentService.UpdateAsync(document);



                if (!continueEditing)
                    return RedirectToAction("~/Plugins/Widgets.BetterDocs/Views/Configure.cshtml");

                return RedirectToAction("~/Plugins/Widgets.BetterDocs/Views/Edit.cshtml", new { id = document.Id });
            }

            model = await _documentModelFactory.PrepareDocumentModelAsync(model, document);

            return View("~/Plugins/Widgets.BetterDocs/Views/Edit.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Delete(int id)
        {

            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return RedirectToAction("~/Plugins/Widgets.BetterDocs/Views/Configure.cshtml");

            await _documentService.DeleteDocumentAsync(document);

            

            return RedirectToAction("~/Plugins/Widgets.BetterDocs/Views/Configure.cshtml");
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
        public virtual async Task<IActionResult> ProductList(ProductDocumentSearchModel searchModel)
        {

            var document = await _documentService.GetDocumentByIdAsync(searchModel.DocumentId)
                ?? throw new ArgumentException("No document found with the specified id");

            var model = await _documentModelFactory.PrepareProductDocumentListModelAsync(searchModel, document);

            return Json(model);
        }

        public virtual async Task<IActionResult> ProductUpdate(ProductDocumentModel model)
        {

            var productDocument = await _documentService.GetProductDocumentByIdAsync(model.Id)
                ?? throw new ArgumentException("No product document mapping found with the specified id");

            productDocument = model.ToEntity(productDocument);
            await _documentService.UpdateProductDocumentAsync(productDocument);

            return new NullJsonResult();
        }

        public virtual async Task<IActionResult> ProductDelete(int id)
        {

            var productDocument = await _documentService.GetProductDocumentByIdAsync(id)
                ?? throw new ArgumentException("No product document mapping found with the specified id", nameof(id));

            await _documentService.DeleteProductDocumentAsync(productDocument);

            return new NullJsonResult();
        }

        public virtual async Task<IActionResult> ProductAddPopup(int documentId)
        {

            var model = await _documentModelFactory.PrepareAddProductToDocumentSearchModelAsync(new AddProductToDocumentSearchModel());

            return View("~/Plugins/Widgets.BetterDocs/Views/ProductAddPopup.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ProductAddPopupList(AddProductToDocumentSearchModel searchModel)
        {

            var model = await _documentModelFactory.PrepareAddProductToDocumentListModelAsync(searchModel);

            return Json(model);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public virtual async Task<IActionResult> ProductAddPopup(AddProductToDocumentModel model)
        {

            var selectedProducts = await _productService.GetProductsByIdsAsync(model.SelectedProductIds.ToArray());
            if (selectedProducts.Any())
            {
                var existingProductDocuments = await _documentService.GetProductDocumentsByDocumentIdAsync(model.DocumentId, showHidden: true);
                foreach (var product in selectedProducts)
                {

                    if (_documentService.FindProductDocument(existingProductDocuments, product.Id, model.DocumentId) != null)
                        continue;


                    await _documentService.InsertProductDocumentAsync(new ProductDocument
                    {
                        DocumentId = model.DocumentId,
                        EntityId = product.Id,
                        KeyGroup = "Product",
                        DisplayOrder = 1
                    });
                }
            }

            ViewBag.RefreshPage = true;

            return View("~/Plugins/Widgets.BetterDocs/Views/ProductAddPopup.cshtml", new AddProductToDocumentSearchModel());
        }

    }
}
