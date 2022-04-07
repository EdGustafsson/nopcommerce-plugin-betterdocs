using Nop.Core;
using System;

namespace Wombit.Plugin.Widgets.BetterDocs.Domain
{
    public partial class Document : BaseEntity
    {
        public int DownloadId { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime UploadedOnUTC { get; set; }
        public string UploadedBy { get; set; }

    }
}
