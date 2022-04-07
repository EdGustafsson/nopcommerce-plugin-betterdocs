using Nop.Web.Framework.Models;

namespace Wombit.Plugin.Widgets.BetterDocs.Models
{
    public partial record ProductDocumentSearchModel : BaseSearchModel
    {
        public int DocumentId { get; set; }

    }
}