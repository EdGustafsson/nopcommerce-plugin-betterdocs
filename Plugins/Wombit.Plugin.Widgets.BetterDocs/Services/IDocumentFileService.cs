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
        Task<Document> InsertDocumentAsync(byte[] documentBinary, string mimeType, string seoFilename, bool validateBinary = true);
        Task<Document> InsertDocumentAsync(IFormFile formFile, string defaultFileName = "", string virtualPath = "");
        //Task<byte[]> ValidateDocumentAsync(byte[] documentBinary, string mimeType);
        //Task<(string Url, Document Document)> GetDocumentUrlAsync(Document document);
        Task<byte[]> LoadDocumentFromFileAsync(int documentId, string extension);
        Task<Document> UpdateDocumentAsync(int documentId, byte[] documentBinary, string mimeType,
        string seoFilename, string title = null);

        Task<byte[]> LoadDocumentBinaryAsync(Document document);
    }
}
