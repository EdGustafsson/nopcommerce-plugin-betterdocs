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

namespace Wombit.Plugin.Widgets.BetterDocs.Services
{
    public partial class DocumentService : IDocumentService
    {
        private readonly IRepository<Document> _repository;
        private readonly IEventPublisher _eventPublisher;


        public DocumentService(
            IRepository<Document> repository,
            IEventPublisher eventPublisher)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
        }


        public async Task InsertAsync(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            await _repository.InsertAsync(document);

            await _eventPublisher.EntityInsertedAsync(document);
        }

        public async Task UpdateAsync(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            await _repository.UpdateAsync(document);

            await _eventPublisher.EntityUpdatedAsync(document);
        }

        public async Task DeleteAsync(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            await _repository.DeleteAsync(document);

            await _eventPublisher.EntityDeletedAsync(document);
        }

        public async Task<Document> GetDocumentAsync(int id)
        {
            if (id == 0)
                return null;

            return await _repository.GetByIdAsync(id);
        }

        public async Task<IList<Document>> GetDocumentsAsync()
        {
            // return await _repository.GetAllAsync();

            // fix

            return await _repository.GetAllAsync(query =>
            {
                return query.OrderBy(t => t.DisplayOrder);
            });
        }

        public virtual async Task<IPagedList<Document>> GetAllDocumentsAsync(int downloadId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var rez = await _repository.GetAllAsync(query =>
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
            return await _repository.GetByIdAsync(downloadId);
        }

        public virtual async Task<IList<Document>> GetDocumentsByIdsAsync(int[] documentIds)
        {
            return await _repository.GetByIdsAsync(documentIds, includeDeleted: false);
        }

         public virtual async Task DeleteDocumentAsync(Document document)
        {
            await _repository.DeleteAsync(document);
        }

        public virtual async Task DeleteDocumentsAsync(IList<Document> documents)
        {
            if (documents == null)
                throw new ArgumentNullException(nameof(documents));

            foreach (var document in documents)
                await DeleteDocumentAsync(document);
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

