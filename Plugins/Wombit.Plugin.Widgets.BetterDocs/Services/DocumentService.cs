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


        public DocumentService(
            IRepository<Document> documentRepository,
            IEventPublisher eventPublisher,
            IRepository<Product> productRepository,
            IRepository<DocumentMapping> documentMappingRepository,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService)
        {
            _documentRepository = documentRepository;
            _eventPublisher = eventPublisher;
            _productRepository = productRepository;
            _documentMappingRepository = documentMappingRepository;
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

        //public virtual async Task<Document> GetDocumentAsync(int id)
        //{
        //    if (id == 0)
        //        return null;

        //    return await _documentRepository.GetByIdAsync(id);
        //}

        public virtual async Task<IList<Document>> GetDocumentsAsync()
        {

            return await _documentRepository.GetAllAsync(query =>
            {
                query = query.Where(point => point.Title != null);

                return query.OrderBy(t => t.DisplayOrder);
            });
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

        //public virtual async Task<IList<DocumentMapping>> GetDocumentMappingsByEntityId(int id)
        //{
        //    throw new NotImplementedException();
        //}


        public virtual async Task<IList<DocumentMapping>> GetDocumentMappingsByEntityIdAsync(int entityId, string keyGroup)
        {

            //var query = from dm in _documentMappingRepository.Table
            //            where dm.EntityId == entityId && !d.Deleted
            //            orderby dm.DisplayOrder, dm.Id
            //            select dm;

            //var documentMappingQuery = _documentMappingRepository.Table; ;

            //query = query.Where(pc => documentMappingQuery.Any(dm => dm.DocumentId == pc.DocumentId));


            //return await query.ToPagedListAsync(pageIndex, pageSize);
            if (entityId == 0)
                return null;

            return await _documentMappingRepository.Table.Where(x => x.EntityId == entityId && x.KeyGroup == keyGroup).ToListAsync();
        }

        public virtual async Task<IList<Document>> GetDocumentsByEntityId(int entityId, string keyGroup)
        {

            var documentMappings = await GetDocumentMappingsByEntityIdAsync(entityId, keyGroup);

            //            select dm;

            var documentIds = documentMappings.Select(dm => dm.DocumentId);

            var documents = _documentRepository.Table.Where(d => documentIds.Contains(d.Id)).ToList();

            //var documents = _documentRepository.Table.Where(d => d.Id == documentIds).ToList();

            return documents;



           
            ////if(keyGroup == "Product")
            ////{
            //var query = from d in _documentRepository.Table
            //            join dm in _documentMappingRepository.Table on d.Id equals dm.DocumentId
            //            where dm.EntityId == entityId && dm.KeyGroup == "Product"
            //            orderby dm.DisplayOrder, dm.Id


            //}


            //var documents = await _documentRepository.GetAllAsync(async query =>

            //   query = query.Where(d => 




            //)
        }
    }
}

