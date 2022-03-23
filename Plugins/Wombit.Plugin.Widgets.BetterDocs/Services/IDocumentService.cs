using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wombit.Plugin.Widgets.BetterDocs.Domain;

namespace Wombit.Plugin.Widgets.BetterDocs.Services
{
    public interface IDocumentService
    {
        Task DeleteAsync(Document document);
        Task InsertAsync(Document document);
        Task UpdateAsync(Document document);
        Task<Document> GetDocumentAsync(int id);
        Task<IList<Document>> GetDocumentsAsync();
    }
}

