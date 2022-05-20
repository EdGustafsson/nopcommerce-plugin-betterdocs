using Nop.Web.Framework.Models;

namespace Wombit.Plugin.Widgets.BetterDocs.Areas.Admin.Models
{
    public partial record DocumentMappingSearchModel : BaseSearchModel
    {
        public int DocumentId { get; set; }

    }
}