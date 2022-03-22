using Nop.Core;
using System;

namespace Wombit.Plugin.Widgets.BetterDocs.Domain
{
    public class Document : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime UploadedOnUTC { get; set; }
        public string UploadedBy { get; set; }

    }
}
