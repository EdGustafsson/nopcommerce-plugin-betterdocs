using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wombit.Plugin.Widgets.BetterDocs.Domain;

namespace Wombit.Plugin.Widgets.BetterDocs.Services
{
    public partial interface IDocumentFileService
    {
        Task<Document> InsertDocumentAsync(byte[] documentBinary, string title, DateTime uploadedOnUTC, string uploadedBy, int displayOrder, string contentType, string extension, string seoFilename, bool validateBinary = true);
        Task<Document> InsertDocumentAsync(IFormFile formFile, string title, DateTime uploadedOnUTC, string uploadedBy, int displayOrder, string defaultFileName = "", string virtualPath = "");
        //Task<byte[]> ValidateDocumentAsync(byte[] documentBinary, string mimeType);
        //Task<(string Url, Document Document)> GetDocumentUrlAsync(Document document);
        Task<byte[]> LoadDocumentFromFileAsync(int documentId, string extension);
        Task<Document> UpdateDocumentInfoAsync(int documentId, byte[] documentBinary, string contentType,
        string seoFilename, string title);

        Task<byte[]> LoadDocumentBinaryAsync(Document document);
        Task<Document> UpdateDocumentAsync(int documentId, IFormFile formFile, string title, DateTime uploadedOnUTC, string uploadedBy, int displayOrder, string defaultFileName = "", string virtualPath = "");

        Task<string> GetFileExtensionFromContentTypeAsync(string contentType);
        Task<Document> UpdateDocumentAsync(int documentId, byte[] documentBinary, string title, DateTime uploadedOnUTC, string uploadedBy, int displayOrder, string contentType, string extension, string seoFilename, bool validateBinary = true);
    }
}
