using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Wombit.Plugin.Widgets.BetterDocs.Domain;
using Wombit.Plugin.Widgets.BetterDocs.Models;


namespace Wombit.Plugin.Widgets.Documents.Infrastructure.Mapper
{
    public class AdminMapperConfiguration : Profile, IOrderedMapperProfile
    {

        public AdminMapperConfiguration()
        {
            CreateMap<Document, DocumentModel>();
            CreateMap<DocumentModel, Document>();
            CreateMap<DocumentMappingModel, DocumentMapping>();
            CreateMap<DocumentMapping, DocumentMappingModel>();
        }


        public int Order => 0;
    }
}
