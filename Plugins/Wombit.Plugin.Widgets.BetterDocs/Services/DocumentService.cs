using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Wombit.Plugin.Widgets.BetterDocs.Domain;
using Nop.Core.Events;
using Nop.Services.Events;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using Microsoft.AspNetCore.Http;
using Nop.Core.Infrastructure;
using SkiaSharp;
using Nop.Services.Media;
using Nop.Core.Domain.Media;
using Nop.Services.Configuration;

namespace Wombit.Plugin.Widgets.BetterDocs.Services
{
    public partial class DocumentService : IDocumentService
    {
        private readonly IRepository<Document> _documentRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<DocumentMapping> _documentMappingRepository;
        private readonly IStoreContext _storeContext;
        private readonly IStoreMappingService _storeMappingService;
        private readonly INopFileProvider _fileProvider;
        private readonly IDownloadService _downloadService;
        private readonly ISettingService _settingService;


        public DocumentService(
            IRepository<Document> documentRepository,
            IEventPublisher eventPublisher,
            IRepository<Product> productRepository,
            IRepository<DocumentMapping> documentMappingRepository,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService,
            INopFileProvider fileProvider,
            IDownloadService downloadService,
            ISettingService settingService
            )
        {
            _documentRepository = documentRepository;
            _eventPublisher = eventPublisher;
            _productRepository = productRepository;
            _documentMappingRepository = documentMappingRepository;
            _storeContext = storeContext;
            _storeMappingService = storeMappingService;
            _fileProvider = fileProvider;
            _downloadService = downloadService;
            _settingService = settingService;
        }

        public virtual async Task<IPagedList<Document>> GetAllDocumentsAsync(int id = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var rez = await _documentRepository.GetAllAsync(query =>
            {
                if (id > 0)
                    query = query.Where(point => point.Id == id || point.Id == 0);
                query = query.OrderBy(point => point.DisplayOrder).ThenBy(point => point.Title).Where(point => point.Title != null);

                return query;
            });

            return new PagedList<Document>(rez, pageIndex, pageSize);
        }

        public virtual async Task<Document> GetDocumentByIdAsync(int id)
        {
            return await _documentRepository.GetByIdAsync(id);
        }

        public virtual async Task<IList<Document>> GetDocumentsByIdsAsync(int[] documentIds)
        {
            return await _documentRepository.GetByIdsAsync(documentIds, includeDeleted: false);
        }
        public virtual async Task<Document> InsertDocumentAsync(IFormFile formFile, string title, DateTime uploadedOnUTC, string uploadedBy, int displayOrder, string defaultFileName = "", string virtualPath = "")
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

            var document = await InsertDocumentAsync(await _downloadService.GetDownloadBitsAsync(formFile), title, uploadedOnUTC, uploadedBy, displayOrder, contentType, fileExtension, _fileProvider.GetFileNameWithoutExtension(fileName));

            if (string.IsNullOrEmpty(virtualPath))
                return document;

            return document;
        }

        public virtual async Task<Document> InsertDocumentAsync(byte[] documentBinary, string title, DateTime uploadedOnUTC, string uploadedBy, int displayOrder, string contentType, string extension, string seoFilename, bool validateBinary = true)
        {
            contentType = CommonHelper.EnsureNotNull(contentType);
            contentType = CommonHelper.EnsureMaximumLength(contentType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            var document = new Document
            {
                Title = title,
                UploadedOnUTC = uploadedOnUTC,
                UploadedBy = uploadedBy,
                DisplayOrder = displayOrder,
                ContentType = contentType,
                SeoFilename = seoFilename,
                Extension = extension,
            };

            await _documentRepository.InsertAsync(document);

            await SaveDocumentInFileAsync(document.Id, documentBinary, contentType);

            return document;
        }
        public virtual async Task<Document> UpdateDocumentInfoAsync(Document document, byte[] documentBinary)
        {
            if (document == null)
                return null;

            var seoFilename = CommonHelper.EnsureMaximumLength(document.SeoFilename, 100);

            document.SeoFilename = seoFilename;

            await _documentRepository.UpdateAsync(document);

            await SaveDocumentInFileAsync(document.Id, documentBinary, document.ContentType);

            return document;
        }

        public virtual async Task<Document> UpdateDocumentInfoAsync(int documentId, byte[] documentBinary, string contentType,
         string seoFilename, string title)
        {
            contentType = CommonHelper.EnsureNotNull(contentType);
            contentType = CommonHelper.EnsureMaximumLength(contentType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            var document = await GetDocumentByIdAsync(documentId);
            if (document == null)
                return null;

            document.ContentType = contentType;
            document.SeoFilename = seoFilename;
            document.Title = title;

            await _documentRepository.UpdateAsync(document);

            await SaveDocumentInFileAsync(document.Id, documentBinary, contentType);

            return document;
        }

        public virtual async Task<Document> UpdateDocumentAsync(int documentId, IFormFile formFile, string title, DateTime uploadedOnUTC, string uploadedBy, int displayOrder, string defaultFileName = "", string virtualPath = "")
        {
            var fileName = formFile.FileName;
            if (string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(defaultFileName))
                fileName = defaultFileName;

            fileName = _fileProvider.GetFileName(fileName);

            var contentType = formFile.ContentType;

            var fileExtension = _fileProvider.GetFileExtension(fileName);
            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            var document = await UpdateDocumentAsync(documentId, await _downloadService.GetDownloadBitsAsync(formFile), title, uploadedOnUTC, uploadedBy, displayOrder, contentType, fileExtension, _fileProvider.GetFileNameWithoutExtension(fileName));

            if (string.IsNullOrEmpty(virtualPath))
                return document;

            return document;
        }
        public virtual async Task<Document> UpdateDocumentAsync(int documentId, byte[] documentBinary, string title, DateTime uploadedOnUTC, string uploadedBy, int displayOrder, string contentType, string extension, string seoFilename, bool validateBinary = true)
        {
            contentType = CommonHelper.EnsureNotNull(contentType);
            contentType = CommonHelper.EnsureMaximumLength(contentType, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);


            var document = await GetDocumentByIdAsync(documentId);
            if (document == null)
                return null;

            await DeletePictureFromFileSystemAsync(document);

            document.ContentType = contentType;
            document.SeoFilename = seoFilename;
            document.Title = title;
            document.UploadedOnUTC = uploadedOnUTC;
            document.UploadedBy = uploadedBy;
            document.DisplayOrder = displayOrder;
            document.Extension = extension;

            await _documentRepository.UpdateAsync(document);

            await SaveDocumentInFileAsync(document.Id, documentBinary, contentType);

            return document;
        }

        public virtual async Task DeleteDocumentAsync(Document document)
        {
            await DeletePictureFromFileSystemAsync(document);

            await _documentRepository.DeleteAsync(document);
        }

        public virtual async Task DeleteDocumentsAsync(IList<Document> documents)
        {
            if (documents == null)
                throw new ArgumentNullException(nameof(documents));

            foreach (var document in documents)
                await DeleteDocumentAsync(document);
        }

        protected virtual async Task SaveDocumentInFileAsync(int documentId, byte[] documentBinary, string contentType)
        {
            var lastPart = await GetFileExtensionFromContentTypeAsync(contentType);
            var fileName = $"{documentId:0000000}_0.{lastPart}";
            await _fileProvider.WriteAllBytesAsync(await GetDocumentLocalPathAsync(fileName), documentBinary);
        }
        public virtual async Task<byte[]> LoadDocumentFromFileAsync(int documentId, string contentType)
        {
            var lastPart = await GetFileExtensionFromContentTypeAsync(contentType);
            var fileName = $"{documentId:0000000}_0.{lastPart}";
            var filePath = await GetDocumentLocalPathAsync(fileName);

            return await _fileProvider.ReadAllBytesAsync(filePath);
        }
        public virtual async Task DeletePictureFromFileSystemAsync(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            var lastPart = await GetFileExtensionFromContentTypeAsync(document.ContentType);
            var fileName = $"{document.Id:0000000}_0.{lastPart}";
            var filePath = await GetDocumentLocalPathAsync(fileName);
            _fileProvider.DeleteFile(filePath);
        }
        public virtual async Task<byte[]> LoadDocumentBinaryAsync(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));


            return await LoadDocumentFromFileAsync(document.Id, document.ContentType); ;
        }

        protected virtual Task<string> GetDocumentLocalPathAsync(string fileName)
        {

            var settings = _settingService.GetAllSettingsAsync().Result;

            var searchString = "betterdocs.location";

            var resultSettings = settings.First(m => m.Name.StartsWith(searchString));


            

            if(resultSettings == null)
            {
                


                return Task.FromResult(_fileProvider.GetAbsolutePath("files", fileName));
            }
            else
            {

                if (!_fileProvider.DirectoryExists(resultSettings.Value))
                {
                    _fileProvider.CreateDirectory(resultSettings.Value);

                }

                return Task.FromResult(_fileProvider.GetAbsolutePath(resultSettings.Value, fileName));
            }

           
        }
        protected virtual Task<string> GetFileExtensionFromContentTypeAsync(string contentType)
        {
            if (contentType == null)
                return Task.FromResult<string>(null);

            var parts = contentType.Split('/');
            var lastPart = parts[^1];

            return Task.FromResult(lastPart);
        }


        public virtual async Task<IPagedList<DocumentMapping>> GetDocumentMappingsByDocumentIdAsync(int documentId, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            if (documentId == 0)
                return new PagedList<DocumentMapping>(new List<DocumentMapping>(), pageIndex, pageSize);

            var query = from dm in _documentMappingRepository.Table
                        join p in _productRepository.Table on dm.EntityId equals p.Id
                        where dm.DocumentId == documentId && !p.Deleted
                        orderby dm.DisplayOrder, dm.Id
                        select dm;

            if (!showHidden)
            {
                var documentsQuery = _documentRepository.Table.Where(d => d.Published); ;

                query = query.Where(pc => documentsQuery.Any(d => d.Id == pc.DocumentId));
            }

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        public virtual async Task<DocumentMapping> GetDocumentMappingByIdAsync(int documentMappingId)
        {
            return await _documentMappingRepository.GetByIdAsync(documentMappingId, cache => default);
        }

        public virtual async Task UpdateDocumentMappingAsync(DocumentMapping documentMapping)
        {
            await _documentMappingRepository.UpdateAsync(documentMapping);
        }

        public virtual async Task DeleteDocumentMappingAsync(DocumentMapping documentMapping)
        {
            await _documentMappingRepository.DeleteAsync(documentMapping);
        }

        public virtual DocumentMapping FindDocumentMapping(IList<DocumentMapping> source, int productId, int documentId)
        {
            foreach (var documentMapping in source)
                if (documentMapping.EntityId == productId && documentMapping.DocumentId == documentId)
                    return documentMapping;

            return null;
        }

        public virtual async Task InsertDocumentMappingAsync(DocumentMapping documentMapping)
        {
            await _documentMappingRepository.InsertAsync(documentMapping);
        }

        public virtual async Task<IList<DocumentMapping>> GetDocumentMappingsByEntityIdAsync(int entityId, string keyGroup)
        {
            if (entityId == 0)
                return null;

            return await _documentMappingRepository.Table.Where(x => x.EntityId == entityId && x.KeyGroup == keyGroup).ToListAsync();
        }

        public virtual async Task<IList<Document>> GetDocumentsByMappingEntityId(int entityId, string keyGroup)
        {

            var documentMappings = await GetDocumentMappingsByEntityIdAsync(entityId, keyGroup);

            var documentIds = documentMappings.Select(dm => dm.DocumentId);

            var documents = _documentRepository.Table.Where(d => documentIds.Contains(d.Id)).ToList();

            return documents;

        }
    }
}

