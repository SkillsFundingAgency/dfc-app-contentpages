using AutoMapper;
using DFC.App.ContentPages.Data.Models;
using DFC.App.ContentPages.Data.ServiceBusModels;

namespace DFC.App.ContentPages.MessageFunctionApp.AutoMapperProfile
{
    public class ContentPageProfile : Profile
    {
        public ContentPageProfile()
        {
            CreateMap<ContentPageMessage, ContentPageModel>()
                .ForMember(d => d.DocumentId, s => s.MapFrom(a => a.ContentPageId))
                .ForPath(d => d.MetaTags.Title, o => o.MapFrom(s => s.Title))
                .ForPath(d => d.MetaTags.Description, o => o.MapFrom(s => s.Description))
                .ForPath(d => d.MetaTags.Keywords, o => o.MapFrom(s => s.Keywords))
                .ForMember(d => d.LastReviewed, o => o.MapFrom(s => s.LastModified))
                .ForMember(d => d.SequenceNumber, o => o.Ignore())
                ;
        }
    }
}