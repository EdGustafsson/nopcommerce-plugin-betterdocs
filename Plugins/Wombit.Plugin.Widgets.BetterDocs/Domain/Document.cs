using Nop.Core;
using System;

namespace Wombit.Plugin.Widgets.BetterDocs.Domain
{
    public partial class Document : BaseEntity
    {
        //public int DownloadId { get; set; }
        public string Title { get; set; }
        public string SeoFilename { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public DateTime UploadedOnUTC { get; set; }
        public string UploadedBy { get; set; }

        public string VirtualPath { get; set; }

    }
}
