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

namespace Wombit.Plugin.Widgets.BetterDocs.Services
{
    public partial class DocumentService : IDocumentService
    {
        private readonly IRepository<Document> _documentRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductDocument> _productDocumentRepository;
        private readonly IStoreContext _storeContext;
        private readonly IStoreMappingService _storeMappingService;


        public DocumentService(
            IRepository<Document> documentRepository,
            IEventPublisher eventPublisher,
            IRepository<Product> productRepository,
            IRepository<ProductDocument> productDocumentRepository,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService)
        {
            _documentRepository = documentRepository;
            _eventPublisher = eventPublisher;
            _productRepository = productRepository;
            _productDocumentRepository = productDocumentRepository;
            _storeContext = storeContext;
            _storeMappingService = storeMappingService;
        }


        public virtual async Task InsertAsync(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            await _documentRepository.InsertAsync(document);

            await _eventPublisher.EntityInsertedAsync(document);
        }

        public virtual async Task UpdateAsync(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            await _documentRepository.UpdateAsync(document);

            await _eventPublisher.EntityUpdatedAsync(document);
        }

        public virtual async Task DeleteAsync(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            await _documentRepository.DeleteAsync(document);

            await _eventPublisher.EntityDeletedAsync(document);
        }

        public virtual async Task<Document> GetDocumentAsync(int id)
        {
            if (id == 0)
                return null;

            return await _documentRepository.GetByIdAsync(id);
        }

        public virtual async Task<IList<Document>> GetDocumentsAsync()
        {
            // return await _repository.GetAllAsync();

            // fix

            return await _documentRepository.GetAllAsync(query =>
            {
                return query.OrderBy(t => t.DisplayOrder);
            });
        }

        public virtual async Task<IPagedList<Document>> GetAllDocumentsAsync(int downloadId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var rez = await _documentRepository.GetAllAsync(query =>
            {
                if (downloadId > 0)
                    query = query.Where(point => point.DownloadId == downloadId || point.Id == 0);
                query = query.OrderBy(point => point.DisplayOrder).ThenBy(point => point.Title);

                return query;
            });

            return new PagedList<Document>(rez, pageIndex, pageSize);
        }

        public virtual async Task<Document> GetDocumentByIdAsync(int downloadId)
        {
            return await _documentRepository.GetByIdAsync(downloadId);
        }

        public virtual async Task<IList<Document>> GetDocumentsByIdsAsync(int[] documentIds)
        {
            return await _documentRepository.GetByIdsAsync(documentIds, includeDeleted: false);
        }

         public virtual async Task DeleteDocumentAsync(Document document)
        {
            await _documentRepository.DeleteAsync(document);
        }

        public virtual async Task DeleteDocumentsAsync(IList<Document> documents)
        {
            if (documents == null)
                throw new ArgumentNullException(nameof(documents));

            foreach (var document in documents)
                await DeleteDocumentAsync(document);
        }

        public virtual async Task<IPagedList<ProductDocument>> GetProductDocumentsByDocumentIdAsync(int documentId, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            if (documentId == 0)
                return new PagedList<ProductDocument>(new List<ProductDocument>(), pageIndex, pageSize);

            var query = from pd in _productDocumentRepository.Table
                        join p in _productRepository.Table on pd.EntityId equals p.Id
                        where pd.DocumentId == documentId && !p.Deleted
                        orderby pd.DisplayOrder, pd.Id
                        select pd;

            if (!showHidden)
            {
                var documentsQuery = _documentRepository.Table.Where(d => d.Published);

                ////apply store mapping constraints
                //var store = await _storeContext.GetCurrentStoreAsync();
                //documentsQuery = await _storeMappingService.ApplyStoreMapping(documentsQuery, store.Id);

                ////apply ACL constraints
                //var customer = await _workContext.GetCurrentCustomerAsync();
                //categoriesQuery = await _aclService.ApplyAcl(documentsQuery, customer);

                query = query.Where(pc => documentsQuery.Any(c => c.Id == pc.DocumentId));
            }

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        public virtual async Task<ProductDocument> GetProductDocumentByIdAsync(int productDocumentId)
        {
            return await _productDocumentRepository.GetByIdAsync(productDocumentId, cache => default);
        }

        public virtual async Task UpdateProductDocumentAsync(ProductDocument productDocument)
        {
            await _productDocumentRepository.UpdateAsync(productDocument);
        }

        public virtual async Task DeleteProductDocumentAsync(ProductDocument productDocument)
        {
            await _productDocumentRepository.DeleteAsync(productDocument);
        }

        public virtual ProductDocument FindProductDocument(IList<ProductDocument> source, int productId, int documentId)
        {
            foreach (var productDocument in source)
                if (productDocument.EntityId == productId && productDocument.DocumentId == documentId)
                    return productDocument;

            return null;
        }

        public virtual async Task InsertProductDocumentAsync(ProductDocument productDocument)
        {
            await _productDocumentRepository.InsertAsync(productDocument);
        }


        //public virtual async Task InsertStorePickupPointAsync(StorePickupPoint pickupPoint)
        //{
        //    await _storePickupPointRepository.InsertAsync(pickupPoint, false);
        //    await _staticCacheManager.RemoveByPrefixAsync(PICKUP_POINT_PATTERN_KEY);
        //}

        //public virtual async Task UpdateStorePickupPointAsync(StorePickupPoint pickupPoint)
        //{
        //    await _storePickupPointRepository.UpdateAsync(pickupPoint, false);
        //    await _staticCacheManager.RemoveByPrefixAsync(PICKUP_POINT_PATTERN_KEY);
        //}

        //public virtual async Task DeleteStorePickupPointAsync(StorePickupPoint pickupPoint)
        //{
        //    await _storePickupPointRepository.DeleteAsync(pickupPoint, false);
        //    await _staticCacheManager.RemoveByPrefixAsync(PICKUP_POINT_PATTERN_KEY);
        //}
    }
}

