using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wombit.Plugin.Widgets.BetterDocs.Domain;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Microsoft.AspNetCore.Http;


namespace Wombit.Plugin.Widgets.BetterDocs.Services
{
    public partial interface IDocumentService
    {
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
        Task<IList<Document>> GetDocumentsByMappingEntityId(int id, string keyGroup);
        Task<Document> InsertDocumentAsync(IFormFile formFile, string title, bool published, DateTime uploadedOnUTC, int uploadedBy, int displayOrder, string defaultFileName = "", string virtualPath = "");
        Task<Document> InsertDocumentAsync(byte[] documentBinary, string title, bool published, DateTime uploadedOnUTC, int uploadedBy, int displayOrder, string contentType, string extension, string seoFilename, bool validateBinary = true);
        Task<Document> UpdateDocumentInfoAsync(int documentId, byte[] documentBinary, string contentType,
        string seoFilename, string title, bool published);
        Task<Document> UpdateDocumentAsync(int documentId, IFormFile formFile, string title, bool published, DateTime uploadedOnUTC, int uploadedBy, int displayOrder, string defaultFileName = "", string virtualPath = "");
        Task<Document> UpdateDocumentAsync(int documentId, byte[] documentBinary, string title, bool published, DateTime uploadedOnUTC, int uploadedBy, int displayOrder, string contentType, string extension, string seoFilename, bool validateBinary = true);
        Task<byte[]> LoadDocumentBinaryAsync(Document document);
        Task<byte[]> LoadDocumentFromFileAsync(int documentId, string extension);
        Task DeleteDocumentFromFileSystemAsync(Document document);
    }
}

