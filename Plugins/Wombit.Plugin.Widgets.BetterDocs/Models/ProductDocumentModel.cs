using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Wombit.Plugin.Widgets.BetterDocs.Models
{
    public partial record ProductDocumentModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Wombit.Document.Fields.DocumentId")]
        public int DocumentId { get; set; }
        [NopResourceDisplayName("Wombit.Document.Fields.ProductName")]
        public int EntityId { get; set; }
        [NopResourceDisplayName("Wombit.Document.Fields.KeyGroup")]
        public string ProductName { get; set; }
        [NopResourceDisplayName("Wombit.Document.Fields.EntityId")]
        public string KeyGroup { get; set; }
        [NopResourceDisplayName("Wombit.Document.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}