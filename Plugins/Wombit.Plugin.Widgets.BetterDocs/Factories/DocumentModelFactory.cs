using System;
using System.Linq;
using System.Threading.Tasks;
using Wombit.Plugin.Widgets.BetterDocs.Models;
using Wombit.Plugin.Widgets.BetterDocs.Services;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Web.Framework.Models.Extensions;

namespace Wombit.Plugin.Widgets.BetterDocs.Factories
{

    public class DocumentModelFactory : IDocumentModelFactory
    {
        #region Fields

        private readonly IDocumentService _documentService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;

        #endregion

        #region Ctor

        public DocumentModelFactory(IDocumentService documentService,
            ILocalizationService localizationService, IStoreService storeService)
        {
            _documentService = documentService;
            _localizationService = localizationService;
            _storeService = storeService;
        }

        #endregion

        #region Methods


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

        #endregion
    }
}
