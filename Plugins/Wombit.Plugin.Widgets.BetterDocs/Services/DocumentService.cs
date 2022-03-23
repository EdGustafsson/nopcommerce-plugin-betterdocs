using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Wombit.Plugin.Widgets.BetterDocs.Domain;
using Nop.Core.Events;
using Nop.Services.Events;

namespace Wombit.Plugin.Widgets.BetterDocs.Services
{
    public class DocumentService : IDocumentService
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

            return await _repository.GetAllAsync(async query =>
            {
                return query.OrderBy(t => t.DisplayOrder);
            });
        }
    }
}

