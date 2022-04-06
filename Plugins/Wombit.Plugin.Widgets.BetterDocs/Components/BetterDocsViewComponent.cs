using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;


namespace Wombit.Plugin.Widgets.BetterDocs.Component
{
    [ViewComponent(Name = "BetterDocs")]
    public class BetterDocsViewComponent : NopViewComponent
    {

        private readonly ISettingService _settingService;
        public BetterDocsViewComponent(
            ISettingService settingService
            )
        {
            _settingService = settingService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {

            var models = new List<string>();

            //foreach (var setting in resultSettings)
            //{
            //    var model = new PublicInfoModel()
            //    {
            //        DisplayText = setting.Value
            //    };

            //    models.Add(model);
            //}

            return View("~/Plugins/Widgets.BetterDocs/Areas/Admin//Views/Default.cshtml", models);

        }
    }
}
