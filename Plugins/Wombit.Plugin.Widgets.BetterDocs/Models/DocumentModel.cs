using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Wombit.Plugin.Widgets.BetterDocs.Models
{
    public record DocumentModel : BaseNopEntityModel
    {
        public DocumentModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            DocumentMappingSearchModel = new DocumentMappingSearchModel();
        }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.Title.Input")]
        public string Title { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.FileName.Input")]
        public string SeoFilename { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.ContentType")]
        public string ContentType { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.Extension")]
        public string Extension { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.UploadedOnUTC")]
        public DateTime UploadedOnUTC { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.UploadedBy")]
        public int UploadedBy { get; set; }
        public DocumentMappingSearchModel DocumentMappingSearchModel { get; set; }


        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.PageSize")]
        public int PageSize { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.AllowCustomersToSelectPageSize")]
        public bool AllowCustomersToSelectPageSize { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.PageSizeOptions")]
        public string PageSizeOptions { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.Published")]
        public bool Published { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.FileIncludeInTopMenuName")]
        public bool IncludeInTopMenu { get; set; }
    }
   
}
