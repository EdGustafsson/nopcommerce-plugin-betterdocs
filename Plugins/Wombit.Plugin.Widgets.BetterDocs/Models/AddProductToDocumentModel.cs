using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Wombit.Plugin.Widgets.BetterDocs.Models
{

    public partial record AddProductToDocumentModel : BaseNopModel
    {

        public AddProductToDocumentModel()
        {
            SelectedProductIds = new List<int>();
        }

        public int DocumentId { get; set; }

        public IList<int> SelectedProductIds { get; set; }

    }
}