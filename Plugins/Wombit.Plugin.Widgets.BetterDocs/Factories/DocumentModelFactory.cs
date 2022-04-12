﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Wombit.Plugin.Widgets.BetterDocs.Models;
using Wombit.Plugin.Widgets.BetterDocs.Services;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Web.Framework.Models.Extensions;
using Wombit.Plugin.Widgets.BetterDocs.Domain;
using Nop.Services.Catalog;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;

namespace Wombit.Plugin.Widgets.BetterDocs.Factories
{

    public class DocumentModelFactory : IDocumentModelFactory
    {

        private readonly IDocumentService _documentService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;

        public DocumentModelFactory(IDocumentService documentService,
            ILocalizationService localizationService, IStoreService storeService, IProductService productService)
        {
            _documentService = documentService;
            _localizationService = localizationService;
            _storeService = storeService;
            _productService = productService;
        }

        protected virtual ProductDocumentSearchModel PrepareProductDocumentSearchModel(ProductDocumentSearchModel searchModel, Document document)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (document == null)
                throw new ArgumentNullException(nameof(document));

            searchModel.DocumentId = document.Id;

            searchModel.SetGridPageSize();

            return searchModel;
        }



        public async Task<DocumentListModel> PrepareDocumentListModelAsync(DocumentSearchModel searchModel)
        {
            var documents = await _documentService.GetAllDocumentsAsync(pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);
            var model = await new DocumentListModel().PrepareToGridAsync(searchModel, documents, () =>
            {
                return documents.SelectAwait(async doc =>
                {
                    var document = await _documentService.GetDocumentByIdAsync(doc.DownloadId);

                    return new DocumentModel
                    {
                        Id = doc.Id,
                        Title = doc.Title,
                        FileName = doc.FileName,
                        DisplayOrder = doc.DisplayOrder,
                        UploadedOnUTC = doc.UploadedOnUTC,
                        UploadedBy = doc.UploadedBy

                    };
                });
            });

            return model;
        }



        public Task<DocumentSearchModel> PrepareDocumentSearchModelAsync(DocumentSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return Task.FromResult(searchModel);
        }
        public virtual async Task<ProductDocumentListModel> PrepareProductDocumentListModelAsync(ProductDocumentSearchModel searchModel, Document document)
        {

            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (document == null)
                throw new ArgumentNullException(nameof(document));


            var productDocuments = await _documentService.GetProductDocumentsByDocumentIdAsync(document.Id,
                showHidden: true,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);


            var model = await new ProductDocumentListModel().PrepareToGridAsync(searchModel, productDocuments, () =>
            {
                return productDocuments.SelectAwait(async productDocument =>
                {
                    var documentProductModel = productDocument.ToModel<ProductDocumentModel>();

                    documentProductModel.ProductName = (await _productService.GetProductByIdAsync(productDocument.EntityId))?.Name;

                    return documentProductModel;
                });
            });

            return model;
        }

        public virtual async Task<AddProductToDocumentSearchModel> PrepareAddProductToDocumentSearchModelAsync(AddProductToDocumentSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            ////prepare available categories
            //await _baseAdminModelFactory.PrepareCategoriesAsync(searchModel.AvailableCategories);

            ////prepare available manufacturers
            //await _baseAdminModelFactory.PrepareManufacturersAsync(searchModel.AvailableManufacturers);

            ////prepare available stores
            //await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);

            ////prepare available vendors
            //await _baseAdminModelFactory.PrepareVendorsAsync(searchModel.AvailableVendors);

            ////prepare available product types
            //await _baseAdminModelFactory.PrepareProductTypesAsync(searchModel.AvailableProductTypes);

            //prepare page parameters
            searchModel.SetPopupGridPageSize();

            return searchModel;
        }

        public virtual async Task<AddProductToDocumentSearchModel> PrepareAddProductToDocumentListModelAsync(AddProductToDocumentSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            ////prepare available categories
            //await _baseAdminModelFactory.PrepareCategoriesAsync(searchModel.AvailableCategories);

            ////prepare available manufacturers
            //await _baseAdminModelFactory.PrepareManufacturersAsync(searchModel.AvailableManufacturers);

            ////prepare available stores
            //await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);

            ////prepare available vendors
            //await _baseAdminModelFactory.PrepareVendorsAsync(searchModel.AvailableVendors);

            ////prepare available product types
            //await _baseAdminModelFactory.PrepareProductTypesAsync(searchModel.AvailableProductTypes);

            //prepare page parameters
            searchModel.SetPopupGridPageSize();

            return searchModel;
        }

        public virtual async Task<DocumentModel> PrepareDocumentModelAsync(DocumentModel documentModel, Document document)
        {
            Action<DocumentLocalizedModel, int> localizedModelConfiguration = null;

            if (document != null)
            {
                //fill in model values from the entity
                if (documentModel == null)
                {
                    documentModel = document.ToModel<DocumentModel>();
                }

                //prepare nested search model
                PrepareProductDocumentSearchModel(documentModel.ProductDocumentSearchModel, document);

            
            }

            //set default values for the new model
            //if (document == null)
            //{
            //    documentModel.PageSize = _catalogSettings.DefaultCategoryPageSize;
            //    documentModel.PageSizeOptions = _catalogSettings.DefaultCategoryPageSizeOptions;
            //    documentModel.Published = true;
            //    documentModel.IncludeInTopMenu = true;
            //    documentModel.AllowCustomersToSelectPageSize = true;
            //}

            //model.PrimaryStoreCurrencyCode = (await _currencyService.GetCurrencyByIdAsync(_currencySettings.PrimaryStoreCurrencyId)).CurrencyCode;

            ////prepare localized models
            //if (!excludeProperties)
            //    model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);

            ////prepare available category templates
            //await _baseAdminModelFactory.PrepareCategoryTemplatesAsync(model.AvailableCategoryTemplates, false);

            ////prepare available parent categories
            //await _baseAdminModelFactory.PrepareCategoriesAsync(model.AvailableCategories,
            //    defaultItemText: await _localizationService.GetResourceAsync("Admin.Catalog.Categories.Fields.Parent.None"));

            ////prepare model discounts
            //var availableDiscounts = await _discountService.GetAllDiscountsAsync(DiscountType.AssignedToCategories, showHidden: true);
            //await _discountSupportedModelFactory.PrepareModelDiscountsAsync(model, category, availableDiscounts, excludeProperties);

            ////prepare model customer roles
            //await _aclSupportedModelFactory.PrepareModelCustomerRolesAsync(model, category, excludeProperties);

            ////prepare model stores
            //await _storeMappingSupportedModelFactory.PrepareModelStoresAsync(model, category, excludeProperties);

            return documentModel;
        }
    }
}