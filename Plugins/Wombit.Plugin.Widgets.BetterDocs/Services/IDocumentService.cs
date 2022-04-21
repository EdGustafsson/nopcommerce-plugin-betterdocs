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
        //Task<Document> GetDocumentAsync(int id);
        Task<IList<Document>> GetDocumentsAsync();
        Task<IPagedList<Document>> GetAllDocumentsAsync(int downloadId = 0, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<Document> GetDocumentByIdAsync(int downloadId);
        Task<IList<Document>> GetDocumentsByIdsAsync(int[] documentIds);
        Task DeleteDocumentAsync(Document document);
        Task DeleteDocumentsAsync(IList<Document> documents);
        Task<IPagedList<DocumentMapping>> GetDocumentMappingsByDocumentIdAsync(int documentId,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
        Task<DocumentMapping> GetDocumentMappingByIdAsync(int documentMappingId);
        Task UpdateDocumentMappingAsync(DocumentMapping documentMapping);
        Task DeleteDocumentMappingAsync(DocumentMapping documentMapping);
        DocumentMapping FindDocumentMapping(IList<DocumentMapping> existingDocumentMappings, int productId, int documentId);
        Task InsertDocumentMappingAsync(DocumentMapping documentMapping);
        Task<IList<DocumentMapping>> GetDocumentMappingsByEntityIdAsync(int id, string keyGroup);
        Task<IList<Document>> GetDocumentsByEntityId(int id, string keyGroup);
    }
}

