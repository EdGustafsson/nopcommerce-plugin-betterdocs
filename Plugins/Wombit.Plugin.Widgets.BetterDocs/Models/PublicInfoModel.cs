using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace Wombit.Plugin.Widgets.BetterDocs.Models
{
    public record PublicInfoModel : BaseNopEntityModel
    {
        public string Id { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.Title.Input")]
        public string Title { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.Extension")]
        public string Extension { get; set; }

        public string DownloadUrl = "admin/betterdocs/downloadfile/";
    }
}
