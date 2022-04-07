using Nop.Core;
using System;

namespace Wombit.Plugin.Widgets.BetterDocs.Domain
{
    public partial class ProductDocument : BaseEntity
    {
        public int DocumentId  { get; set; }
        public int EntityId { get; set; }
        public string KeyGroup { get; set; }
        public int DisplayOrder { get; set; }

    }
}
 