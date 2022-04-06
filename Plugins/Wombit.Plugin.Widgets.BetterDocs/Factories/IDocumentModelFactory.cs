using System.Threading.Tasks;
using Wombit.Plugin.Widgets.BetterDocs.Models;

namespace Wombit.Plugin.Widgets.BetterDocs.Factories
{
   
    public interface IDocumentModelFactory
    {
        
        Task<DocumentListModel> PrepareDocumentListModelAsync(DocumentSearchModel searchModel);
        Task<DocumentSearchModel> PrepareDocumentSearchModelAsync(DocumentSearchModel searchModel);
    }
}