using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wombit.Plugin.Widgets.BetterDocs.Domain;
using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Wombit.Plugin.Widgets.BetterDocs.Services
{
    public partial interface IDocumentService
    {
        Task DeleteAsync(Document document);
        Task InsertAsync(Document document);
        Task UpdateAsync(Document document);
        Task<Document> GetDocumentAsync(int id);
        Task<IList<Document>> GetDocumentsAsync();
        Task<IPagedList<Document>> GetAllDocumentsAsync(int downloadId = 0, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<Document> GetDocumentByIdAsync(int downloadId);
        Task<IList<Document>> GetDocumentsByIdsAsync(int[] documentIds);
        Task DeleteDocumentAsync(Document document);
        Task DeleteDocumentsAsync(IList<Document> documents);
        Task<IPagedList<ProductDocument>> GetProductDocumentsByDocumentIdAsync(int documentId,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
        Task<ProductDocument> GetProductDocumentByIdAsync(int productDocumentId);
        Task UpdateProductDocumentAsync(ProductDocument productDocument);
        Task DeleteProductDocumentAsync(ProductDocument productDocument);
        ProductDocument FindProductDocument(IList<ProductDocument> existingProductDocuments, int productId, int documentId);
        Task InsertProductDocumentAsync(ProductDocument productDocument);
    }
}

