using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Wombit.Plugin.Widgets.BetterDocs.Areas.Admin.Models
{

    public partial record AddMappingToDocumentSearchModel : BaseSearchModel
    {

        public AddMappingToDocumentSearchModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableManufacturers = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            AvailableVendors = new List<SelectListItem>();
            AvailableProductTypes = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.SearchProductName")]
        public string SearchProductName { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.SearchCategory")]
        public int SearchCategoryId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.SearchManufacturer")]
        public int SearchManufacturerId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.SearchStore")]
        public int SearchStoreId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.SearchVendor")]
        public int SearchVendorId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.BetterDocs.Admin.Fields.SearchProductType")]
        public int SearchProductTypeId { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        public IList<SelectListItem> AvailableManufacturers { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        public IList<SelectListItem> AvailableVendors { get; set; }

        public IList<SelectListItem> AvailableProductTypes { get; set; }

    }
}