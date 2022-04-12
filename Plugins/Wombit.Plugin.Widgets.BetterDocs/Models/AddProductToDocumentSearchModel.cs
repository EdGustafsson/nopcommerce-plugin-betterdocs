﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Wombit.Plugin.Widgets.BetterDocs.Models
{

    public partial record AddProductToDocumentSearchModel : BaseSearchModel
    {

        public AddProductToDocumentSearchModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableManufacturers = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            AvailableVendors = new List<SelectListItem>();
            AvailableProductTypes = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Wombit.Catalog.Products.List.SearchProductName")]
        public string SearchProductName { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
        public int SearchCategoryId { get; set; }

        [NopResourceDisplayName("Wombit.Catalog.Products.List.SearchManufacturer")]
        public int SearchManufacturerId { get; set; }

        [NopResourceDisplayName("Wombit.Catalog.Products.List.SearchStore")]
        public int SearchStoreId { get; set; }

        [NopResourceDisplayName("Wombit.Catalog.Products.List.SearchVendor")]
        public int SearchVendorId { get; set; }

        [NopResourceDisplayName("Wombit.Catalog.Products.List.SearchProductType")]
        public int SearchProductTypeId { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        public IList<SelectListItem> AvailableManufacturers { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        public IList<SelectListItem> AvailableVendors { get; set; }

        public IList<SelectListItem> AvailableProductTypes { get; set; }

    }
}