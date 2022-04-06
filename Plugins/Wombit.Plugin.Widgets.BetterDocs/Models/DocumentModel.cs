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
