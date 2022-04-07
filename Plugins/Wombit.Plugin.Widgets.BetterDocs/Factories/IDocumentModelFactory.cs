using System.Threading.Tasks;
using Wombit.Plugin.Widgets.BetterDocs.Models;
using Wombit.Plugin.Widgets.BetterDocs.Domain;

namespace Wombit.Plugin.Widgets.BetterDocs.Factories
{
   
    public interface IDocumentModelFactory
    {
        
        Task<DocumentListModel> PrepareDocumentListModelAsync(DocumentSearchModel searchModel);
        Task<DocumentSearchModel> PrepareDocumentSearchModelAsync(DocumentSearchModel searchModel);
        Task<ProductDocumentListModel> PrepareProductDocumentListModelAsync(ProductDocumentSearchModel searchModel, Document document);
        Task<AddProductToDocumentSearchModel> PrepareAddProductToDocumentSearchModelAsync(AddProductToDocumentSearchModel addProductToDocumentSearchModel);
    }
}