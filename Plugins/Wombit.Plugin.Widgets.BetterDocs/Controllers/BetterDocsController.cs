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
using Wombit.Plugin.Widgets.BetterDocs.Areas.Admin.Models;
using Wombit.Plugin.Widgets.BetterDocs.Areas.Admin.Services;

namespace Wombit.Plugin.Widgets.BetterDocs.Controllers
{
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
    }
}

