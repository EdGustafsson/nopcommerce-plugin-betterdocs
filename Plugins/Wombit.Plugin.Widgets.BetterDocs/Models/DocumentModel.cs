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
    public record DocumentModel : BaseNopEntityModel, ILocalizedModel<DocumentLocalizedModel>
    {
        public DocumentModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            Locales = new List<DocumentLocalizedModel>();
          

            ProductDocumentSearchModel = new ProductDocumentSearchModel();
        }

        [UIHint("Download")]
        [NopResourceDisplayName("Wombit.Document.Fields.DownloadId")]
        public int DownloadId { get; set; }
        public Guid DownloadGuid { get; set; }

        [NopResourceDisplayName("Wombit.Document.Fields.Title")]
        public string Title { get; set; }

        [NopResourceDisplayName("Wombit.Document.Fields.FileName")]
        public string FileName { get; set; }

        [NopResourceDisplayName("Wombit.Document.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Wombit.Document.Fields.UploadedOnUTC")]
        public DateTime UploadedOnUTC { get; set; }

        [NopResourceDisplayName("Wombit.Document.Fields.UploadedBy")]
        public string UploadedBy { get; set; }
        public ProductDocumentSearchModel ProductDocumentSearchModel { get; set; }


        [NopResourceDisplayName("Admin.Catalog.Categories.Fields.PageSize")]
        public int PageSize { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Categories.Fields.AllowCustomersToSelectPageSize")]
        public bool AllowCustomersToSelectPageSize { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Categories.Fields.PageSizeOptions")]
        public string PageSizeOptions { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Categories.Fields.Published")]
        public bool Published { get; set; }
        public IList<DocumentLocalizedModel> Locales { get; set; }
        [NopResourceDisplayName("Admin.Catalog.Categories.Fields.IncludeInTopMenu")]
        public bool IncludeInTopMenu { get; set; }
    }

    public partial record DocumentLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [UIHint("Download")]
        [NopResourceDisplayName("Wombit.Document.Fields.DownloadId")]
        public int DownloadId { get; set; }
        public Guid DownloadGuid { get; set; }

        [NopResourceDisplayName("Wombit.Document.Fields.Title")]
        public string Title { get; set; }

        [NopResourceDisplayName("Wombit.Document.Fields.FileName")]
        public string FileName { get; set; }

        [NopResourceDisplayName("Wombit.Document.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Wombit.Document.Fields.UploadedOnUTC")]
        public DateTime UploadedOnUTC { get; set; }

        [NopResourceDisplayName("Wombit.Document.Fields.UploadedBy")]
        public string UploadedBy { get; set; }
    }
}
