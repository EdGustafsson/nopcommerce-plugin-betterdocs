using System.Threading.Tasks;
using Wombit.Plugin.Widgets.BetterDocs.Areas.Admin.Models;
using Wombit.Plugin.Widgets.BetterDocs.Models;
using Wombit.Plugin.Widgets.BetterDocs.Domain;
using System.Collections.Generic;

namespace Wombit.Plugin.Widgets.BetterDocs.Factories
{
   
    public interface IDocumentModelFactory
    {
        
        Task<DocumentListModel> PrepareDocumentListModelAsync(DocumentSearchModel searchModel);
        Task<DocumentSearchModel> PrepareDocumentSearchModelAsync(DocumentSearchModel searchModel);
        Task<DocumentMappingListModel> PrepareDocumentMappingListModelAsync(DocumentMappingSearchModel searchModel, Document document);
        Task<AddMappingToDocumentSearchModel> PrepareAddProductToDocumentSearchModelAsync(AddMappingToDocumentSearchModel addProductToDocumentSearchModel);
        Task<AddMappingToDocumentListModel> PrepareAddProductToDocumentListModelAsync(AddMappingToDocumentSearchModel searchModel);
        Task<DocumentModel> PrepareDocumentModelAsync(DocumentModel documentModel, Document document);
        Task<List<PublicInfoModel>> PreparePublicInfoModelAsync(List<Document> documentList);
    }
}