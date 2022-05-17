using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Wombit.Plugin.Widgets.BetterDocs
{
    public class BetterDocsSettings : ISettings
    {
        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.FileLocation")]
        public string FileLocation { get; set; }
    }
}
