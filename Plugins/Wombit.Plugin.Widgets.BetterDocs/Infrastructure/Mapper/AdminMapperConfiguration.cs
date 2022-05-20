using AutoMapper;
using Nop.Core.Infrastructure.Mapper;
using Wombit.Plugin.Widgets.BetterDocs.Domain;
using Wombit.Plugin.Widgets.BetterDocs.Models;
using Wombit.Plugin.Widgets.BetterDocs.Areas.Admin.Models;


namespace Wombit.Plugin.Widgets.BetterDocs.Infrastructure.Mapper
{
    public class AdminMapperConfiguration : Profile, IOrderedMapperProfile
    {

        public AdminMapperConfiguration()
        {
            CreateMap<Document, DocumentModel>();
            CreateMap<DocumentModel, Document>();
            CreateMap<DocumentMappingModel, DocumentMapping>();
            CreateMap<DocumentMapping, DocumentMappingModel>();

            CreateMap<Document, PublicInfoModel>();
        }


        public int Order => 0;
    }
}
