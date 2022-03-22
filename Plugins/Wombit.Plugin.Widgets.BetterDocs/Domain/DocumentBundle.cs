using Nop.Core;

namespace Wombit.Plugin.Widgets.BetterDocs.Domain
{
    public class DocumentBundle : BaseEntity
    {
        public int Id { get; set; }
        public int DocumentId   { get; set; }
        public int EntityId { get; set; }
        public string KeyGroup { get; set; }
    }
}
