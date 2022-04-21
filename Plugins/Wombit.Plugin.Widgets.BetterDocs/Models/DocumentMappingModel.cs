using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Wombit.Plugin.Widgets.BetterDocs.Models
{
    public partial record DocumentMappingModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.DownloadId")]
        public int DownloadId { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.EntityId")]
        public int EntityId { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.ProductName")]
        public string ProductName { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.KeyGroup")]
        public string KeyGroup { get; set; }
        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
}