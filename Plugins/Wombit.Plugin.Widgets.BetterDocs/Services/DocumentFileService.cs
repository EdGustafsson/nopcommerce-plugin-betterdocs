using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Infrastructure;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wombit.Plugin.Widgets.BetterDocs.Domain;
using Nop.Data;
using Nop.Services.Media;
using Nop.Core.Domain.Media;

namespace Wombit.Plugin.Widgets.BetterDocs.Services
{
    public partial class DocumentFileService : IDocumentFileService
    {

        private readonly INopFileProvider _fileProvider;
        private readonly IRepository<Document> _documentRepository;
        private readonly IDownloadService _downloadService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MediaSettings _mediaSettings;
        private readonly IWebHelper _webHelper;
        private readonly IDocumentService _documentService;
        public DocumentFileService(INopFileProvider fileProvider,
            IRepository<Document> documentRepository,
            IDownloadService downloadService,
            IHttpContextAccessor httpContextAccessor,
            MediaSettings mediaSettings,
            IWebHelper webHelper,
            IDocumentService documentService)
        {
            _fileProvider = fileProvider;
            _documentRepository = documentRepository;
            _downloadService = downloadService;
            _httpContextAccessor = httpContextAccessor;
            _mediaSettings = mediaSettings;
            _webHelper = webHelper;
            _documentService = documentService;
        }

        //public virtual async Task<string> GetDocumentUrlAsync(int documentId)
        //{
        //    var document = await GetDocumentByIdAsync(documentId);
        //    return (await GetDocumentUrlAsync(document).Url);
        //}

        protected virtual async Task<byte[]> LoadDocumentFromFileAsync(int documentId, string extension, int targetSize = 0)
        {
            //var lastPart = await GetFileExtensionFromMimeTypeAsync(mimeType);
            var fileName = $"{documentId:0000000}_0.{extension}";
            var filePath = await GetDocumentLocalPathAsync(fileName);

            return await _fileProvider.ReadAllBytesAsync(filePath);
        }
        


        protected virtual Task<string> GetDocumentsPathUrlAsync(string storeLocation = null)
        {
            var pathBase = _httpContextAccessor.HttpContext.Request.PathBase.Value ?? string.Empty;
            var documentsPathUrl = _mediaSettings.UseAbsoluteImagePath ? storeLocation : $"{pathBase}/";
            documentsPathUrl = string.IsNullOrEmpty(documentsPathUrl) ? _webHelper.GetStoreLocation() : documentsPathUrl;
            documentsPathUrl += "images/";

            return Task.FromResult(documentsPathUrl);
        }

        public virtual async Task<Document> InsertDocumentAsync(IFormFile formFile, string defaultFileName = "", string virtualPath = "")
        {

            var fileName = formFile.FileName;
            if (string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(defaultFileName))
                fileName = defaultFileName;

            //remove path (passed in IE)
            fileName = _fileProvider.GetFileName(fileName);

            var contentType = formFile.ContentType;

            var fileExtension = _fileProvider.GetFileExtension(fileName);
            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            var document = await InsertDocumentAsync(await _downloadService.GetDownloadBitsAsync(formFile), contentType, _fileProvider.GetFileNameWithoutExtension(fileName));

            if (string.IsNullOrEmpty(virtualPath))
                return document;

            document.VirtualPath = _fileProvider.GetVirtualPath(virtualPath);
            await UpdateDocumentAsync(document, await _downloadService.GetDownloadBitsAsync(formFile));

            return document;
        }

        public virtual async Task<Document> InsertDocumentAsync(byte[] documentBinary, string mimeType, string seoFilename, bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            var document = new Document
            {
                MimeType = mimeType,
                SeoFilename = seoFilename,
            };

            await _documentRepository.InsertAsync(document);
            //await UpdatePictureBinaryAsync(picture, await IsStoreInDbAsync() ? pictureBinary : Array.Empty<byte>());

            await SaveDocumentInFileAsync(document.Id, documentBinary, mimeType);

            return document;
        }

        protected virtual async Task SaveDocumentInFileAsync(int documentId, byte[] documentBinary, string mimeType)
        {
            var lastPart = await GetFileExtensionFromMimeTypeAsync(mimeType);
            var fileName = $"{documentId:0000000}_0.{lastPart}";
            await _fileProvider.WriteAllBytesAsync(await GetDocumentLocalPathAsync(fileName), documentBinary);
        }

        protected virtual Task<string> GetDocumentLocalPathAsync(string fileName)
        {
            return Task.FromResult(_fileProvider.GetAbsolutePath("images", fileName));
        }


        public virtual async Task<Document> UpdateDocumentAsync(Document document, byte[] documentBinary)
        {
            if (document == null)
                return null;

            var seoFilename = CommonHelper.EnsureMaximumLength(document.SeoFilename, 100);

            //delete old thumbs if a picture has been changed
            //if (seoFilename != document.SeoFilename)
            //    await DeletePictureThumbsAsync(picture);

            document.SeoFilename = seoFilename;

            await _documentRepository.UpdateAsync(document);
            //await UpdatePictureBinaryAsync(picture, await IsStoreInDbAsync() ? (await GetPictureBinaryByPictureIdAsync(picture.Id)).BinaryData : Array.Empty<byte>());

            await SaveDocumentInFileAsync(document.Id, documentBinary, document.MimeType);

            return document;
        }

        public virtual async Task<byte[]> LoadDocumentFromFileAsync(int documentId, string mimeType)
        {
            var lastPart = await GetFileExtensionFromMimeTypeAsync(mimeType);
            var fileName = $"{documentId:0000000}_0.{lastPart}";
            var filePath = await GetDocumentLocalPathAsync(fileName);

            return await _fileProvider.ReadAllBytesAsync(filePath);
        }

       

        public virtual async Task<Document> UpdateDocumentAsync(int documentId, byte[] documentBinary, string mimeType,
         string seoFilename, string title = null)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            var document = await _documentService.GetDocumentByIdAsync(documentId);
            if (document == null)
                return null;

            ////delete old thumbs if a picture has been changed
            //if (seoFilename != picture.SeoFilename)
            //    await DeletePictureThumbsAsync(picture);

            document.MimeType = mimeType;
            document.SeoFilename = seoFilename;
            document.Title = title;

            await _documentRepository.UpdateAsync(document);

            await SaveDocumentInFileAsync(document.Id, documentBinary, mimeType);

            return document;
        }



        //public virtual async Task<(string Url, Document Picture)> GetPictureUrlAsync(Document document)
        //{


        //    byte[] pictureBinary = null;


        //    var seoFileName = document.SeoFilename; 

        //    //var lastPart = await GetFileExtensionFromMimeTypeAsync(picture.MimeType);

        //    string fileName;
        //    if (targetSize == 0)
        //    {
        //        fileName = !string.IsNullOrEmpty(seoFileName)
        //            ? $"{document.Id:0000000}_{seoFileName}.{document.MimeType}"
        //            : $"{document.Id:0000000}.{document.MimeType}";

        //        var thumbFilePath = await GetDocumentLocalPathAsync(fileName);
        //        //if (await GeneratedThumbExistsAsync(thumbFilePath, thumbFileName))
        //        return (await GetThumbUrlAsync(thumbFileName, storeLocation), picture);

        //        pictureBinary ??= await LoadPictureBinaryAsync(picture);

        //        //the named mutex helps to avoid creating the same files in different threads,
        //        //and does not decrease performance significantly, because the code is blocked only for the specific file.
        //        //you should be very careful, mutexes cannot be used in with the await operation
        //        //we can't use semaphore here, because it produces PlatformNotSupportedException exception on UNIX based systems
        //        using var mutex = new Mutex(false, thumbFileName);
        //        mutex.WaitOne();
        //        try
        //        {
        //            SaveThumbAsync(thumbFilePath, thumbFileName, string.Empty, pictureBinary).Wait();
        //        }
        //        finally
        //        {
        //            mutex.ReleaseMutex();
        //        }
        //    }
        //    else
        //    {
        //        fileName = !string.IsNullOrEmpty(seoFileName)
        //            ? $"{picture.Id:0000000}_{seoFileName}_{targetSize}.{lastPart}"
        //            : $"{picture.Id:0000000}_{targetSize}.{lastPart}";

        //        var thumbFilePath = await GetThumbLocalPathAsync(thumbFileName);
        //        if (await GeneratedThumbExistsAsync(thumbFilePath, thumbFileName))
        //            return (await GetThumbUrlAsync(thumbFileName, storeLocation), picture);

        //        pictureBinary ??= await LoadPictureBinaryAsync(picture);

        //        //the named mutex helps to avoid creating the same files in different threads,
        //        //and does not decrease performance significantly, because the code is blocked only for the specific file.
        //        //you should be very careful, mutexes cannot be used in with the await operation
        //        //we can't use semaphore here, because it produces PlatformNotSupportedException exception on UNIX based systems
        //        using var mutex = new Mutex(false, thumbFileName);
        //        mutex.WaitOne();
        //        try
        //        {
        //            if (pictureBinary != null)
        //            {
        //                try
        //                {
        //                    using var image = SKBitmap.Decode(pictureBinary);
        //                    var format = GetImageFormatByMimeType(picture.MimeType);
        //                    pictureBinary = ImageResize(image, format, targetSize);
        //                }
        //                catch
        //                {
        //                }
        //            }

        //            SaveThumbAsync(thumbFilePath, thumbFileName, string.Empty, pictureBinary).Wait();
        //        }
        //        finally
        //        {
        //            mutex.ReleaseMutex();
        //        }
        //        //}

        //        return (await GetDocuUrlAsync(fileName, storeLocation), document);
        //}

        //protected virtual async Task<string> GetPictureUrlAsync(string pictureFileName, string storeLocation = null)
        //{
        //    var url = await GetPicturesPathUrlAsync(storeLocation) + "thumbs/";

        //    if (_mediaSettings.MultipleThumbDirectories)
        //    {
        //        //get the first two letters of the file name
        //        var fileNameWithoutExtension = _fileProvider.GetFileNameWithoutExtension(thumbFileName);
        //        if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > NopMediaDefaults.MultipleThumbDirectoriesLength)
        //        {
        //            var subDirectoryName = fileNameWithoutExtension[0..NopMediaDefaults.MultipleThumbDirectoriesLength];
        //            url = url + subDirectoryName + "/";
        //        }
        //    }

        //    url += thumbFileName;
        //    return url;
        //}
        //protected virtual async Task<string> GetDocuUrlAsync(string fileName, string storeLocation = null)
        //{
        //    var url = await GetImagesPathUrlAsync(storeLocation) + "thumbs/";

        //    if (_mediaSettings.MultipleThumbDirectories)
        //    {
        //        //get the first two letters of the file name
        //        var fileNameWithoutExtension = _fileProvider.GetFileNameWithoutExtension(thumbFileName);
        //        if (fileNameWithoutExtension != null && fileNameWithoutExtension.Length > NopMediaDefaults.MultipleThumbDirectoriesLength)
        //        {
        //            var subDirectoryName = fileNameWithoutExtension[0..NopMediaDefaults.MultipleThumbDirectoriesLength];
        //            url = url + subDirectoryName + "/";
        //        }
        //    }

        //    url += thumbFileName;
        //    return url;
        //}

        public virtual Task<string> GetFileExtensionFromMimeTypeAsync(string mimeType)
        {
            if (mimeType == null)
                return Task.FromResult<string>(null);

            var parts = mimeType.Split('/');
            var lastPart = parts[^1];
            //switch (lastPart)
            //{
            //    case "pjpeg":
            //        lastPart = "jpg";
            //        break;
            //    case "x-png":
            //        lastPart = "png";
            //        break;
            //    case "x-icon":
            //        lastPart = "ico";
            //        break;
            //    default:
            //        break;
            //}

            return Task.FromResult(lastPart);
        }

        public virtual async Task<byte[]> LoadDocumentBinaryAsync(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));


            return await LoadDocumentFromFileAsync(document.Id, document.MimeType); ;
        }
    }
}
