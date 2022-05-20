using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Wombit.Plugin.Widgets.BetterDocs.Areas.Admin.Models
{

    public partial record AddMappingToDocumentModel : BaseNopModel
    {

        public AddMappingToDocumentModel()
        {
            SelectedProductIds = new List<int>();
        }

        public int DocumentId { get; set; }

        public IList<int> SelectedProductIds { get; set; }

    }
}