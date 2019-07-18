using AutoMapper;
using DFC.App.Help.Data;
using DFC.App.Help.ViewModels;
using Microsoft.AspNetCore.Html;

namespace DFC.App.Help.AutoMapperProfiles
{
    public class HelpPageModelProfile : Profile
    {
        public HelpPageModelProfile()
        {
            CreateMap<HelpPageModel, BodyViewModel>()
                .ForMember(d => d.Content, s => s.MapFrom(a => new HtmlString(a.Content)))
                ;

            CreateMap<HelpPageModel, DocumentViewModel>()
                .ForMember(d => d.Breadcrumb, s => s.Ignore())
                .ForMember(d => d.Content, s => s.MapFrom(a => new HtmlString(a.Content)))
                .ForMember(d => d.Title, s => s.MapFrom(a => a.MetaTags.Title))
                .ForMember(d => d.Description, s => s.MapFrom(a => a.MetaTags.Description))
                .ForMember(d => d.Keywords, s => s.MapFrom(a => a.MetaTags.Keywords))
                ;

            CreateMap<HelpPageModel, HeadViewModel>()
                .ForMember(d => d.CanonicalUrl, s => s.Ignore())
                .ForMember(d => d.Title, s => s.MapFrom(a => a.MetaTags.Title))
                .ForMember(d => d.Description, s => s.MapFrom(a => a.MetaTags.Description))
                .ForMember(d => d.Keywords, s => s.MapFrom(a => a.MetaTags.Keywords))
                ;

            CreateMap<HelpPageModel, IndexDocumentViewModel>()
                ;
        }
    }
}
