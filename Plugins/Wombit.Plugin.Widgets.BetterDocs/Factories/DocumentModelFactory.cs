using System;
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
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Discounts;
using Nop.Services.Directory;
using Nop.Services.Discounts;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Extensions;
using Nop.Web.Framework.Factories;
using Nop.Web.Areas.Admin.Factories;
using Nop.Services.Media;

namespace Wombit.Plugin.Widgets.BetterDocs.Factories
{

    public class DocumentModelFactory : IDocumentModelFactory
    {

        private readonly IDocumentService _documentService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IDownloadService _downloadService;

        public DocumentModelFactory(IDocumentService documentService,
            ILocalizationService localizationService, 
            IStoreService storeService, 
            IProductService productService, 
            IUrlRecordService urlRecordService, 
            IBaseAdminModelFactory baseAdminModelFactory,
            IDownloadService downloadService)
        {
            _documentService = documentService;
            _localizationService = localizationService;
            _storeService = storeService;
            _productService = productService;
            _urlRecordService = urlRecordService;
            _baseAdminModelFactory = baseAdminModelFactory;
            _downloadService = downloadService;
        }

        protected virtual DocumentMappingSearchModel PrepareDocumentMappingSearchModel(DocumentMappingSearchModel searchModel, Document document)
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
                    var document = await _documentService.GetDocumentByIdAsync(doc.Id);

                    return new DocumentModel
                    {
                        Id = doc.Id,
                        Title = doc.Title,
                        SeoFilename = doc.SeoFilename,
                        ContentType = doc.ContentType,
                        Extension = doc.Extension,
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

            searchModel.SetGridPageSize();

            return Task.FromResult(searchModel);
        }
        public virtual async Task<DocumentMappingListModel> PrepareDocumentMappingListModelAsync(DocumentMappingSearchModel searchModel, Document document)
        {

            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (document == null)
                throw new ArgumentNullException(nameof(document));


            var documentMappings = await _documentService.GetDocumentMappingsByDocumentIdAsync(document.Id,
                showHidden: true,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);


            var model = await new DocumentMappingListModel().PrepareToGridAsync(searchModel, documentMappings, () =>
            {
                return documentMappings.SelectAwait(async documentMapping =>
                {
                    var documentProductModel = documentMapping.ToModel<DocumentMappingModel>();

                    documentProductModel.ProductName = (await _productService.GetProductByIdAsync(documentMapping.EntityId))?.Name;

                    return documentProductModel;
                });
            });

            return model;
        }

        public virtual async Task<AddMappingToDocumentSearchModel> PrepareAddProductToDocumentSearchModelAsync(AddMappingToDocumentSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available categories
            await _baseAdminModelFactory.PrepareCategoriesAsync(searchModel.AvailableCategories);

            //prepare available manufacturers
            await _baseAdminModelFactory.PrepareManufacturersAsync(searchModel.AvailableManufacturers);

            //prepare available stores
            await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);

            //prepare available vendors
            await _baseAdminModelFactory.PrepareVendorsAsync(searchModel.AvailableVendors);

            //prepare available product types
            await _baseAdminModelFactory.PrepareProductTypesAsync(searchModel.AvailableProductTypes);

            //prepare page parameters
            searchModel.SetPopupGridPageSize();

            return searchModel;
        }

        public virtual async Task<AddMappingToDocumentListModel> PrepareAddProductToDocumentListModelAsync(AddMappingToDocumentSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var products = await _productService.SearchProductsAsync(showHidden: true,
                categoryIds: new List<int> { searchModel.SearchCategoryId },
                manufacturerIds: new List<int> { searchModel.SearchManufacturerId },
                storeId: searchModel.SearchStoreId,
                vendorId: searchModel.SearchVendorId,
                productType: searchModel.SearchProductTypeId > 0 ? (ProductType?)searchModel.SearchProductTypeId : null,
                keywords: searchModel.SearchProductName,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            var model = await new AddMappingToDocumentListModel().PrepareToGridAsync(searchModel, products, () =>
            {
                return products.SelectAwait(async product =>
                {
                    var productModel = product.ToModel<ProductModel>();

                    productModel.SeName = await _urlRecordService.GetSeNameAsync(product, 0, true, false);

                    return productModel;
                });
            });

            return model;
        }

        public virtual async Task<DocumentModel> PrepareDocumentModelAsync(DocumentModel documentModel, Document document)
        {

            if (document != null)
            {
                if (documentModel == null)
                {
                    documentModel = document.ToModel<DocumentModel>();
                }

                PrepareDocumentMappingSearchModel(documentModel.DocumentMappingSearchModel, document);
  
            }

            return documentModel;
        }
    }
}