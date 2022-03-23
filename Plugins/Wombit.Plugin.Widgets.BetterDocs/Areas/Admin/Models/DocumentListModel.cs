using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Wombit.Plugin.Widgets.BetterDocs.Areas.Admin.Models
{
    public record DocumentListModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Wombit.Document.Fields.Title")]
        public string Title { get; set; }
    }
}
