using AutoMapper;
using ttxaphuong.DTO.Category_field;
using ttxaphuong.Models.Accounts;
using ttxaphuong.Models.News_events;
using ttxaphuong.DTO.Accounts;
using ttxaphuong.DTO.Documents;
using ttxaphuong.Models.Documents;
using ttxaphuong.DTO.News_events;
using ttxaphuong.Models.Procedures;
using ttxaphuong.Models.Introduce;
using ttxaphuong.DTO.Introduces;
using ttxaphuong.DTO.Procedures;
using ttxaphuong.Models.Feedbacks;
using ttxaphuong.DTO.Feedbacks;
using ttxaphuong.Models.Loads;
using ttxaphuong.DTO.Uploads;
using ttxaphuong.DTO;

namespace Infrastructure.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AccountsModel, AccountsDTO>().ReverseMap();

            CreateMap<PermissionsModel, PermissionsDTO>().ReverseMap();

            CreateMap<Categogy_fieldModel, Category_fieldDTO>().ReverseMap();

            CreateMap<Category_documentsModel, Category_documentsDTO>()
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children))
                .ReverseMap();

            CreateMap<News_eventsModel, News_eventsDTO>().ReverseMap();

            CreateMap<CategoriesModel, CategoriesDTO>()
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children))
                .ReverseMap();

            CreateMap<Category_documentsModel, Category_documentsDTO>().ReverseMap();
            CreateMap<DocumentModel, DocumentsDTO>().ReverseMap();

            CreateMap<Categories_introduceModel, Categories_introduceDTO>().ReverseMap();
            CreateMap<IntroduceModel, IntroduceDTO>().ReverseMap();


            CreateMap<Categogy_fieldModel, Category_fieldDTO>()
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children))
                .ReverseMap();

            CreateMap<ProceduresModel, ProceduresDTO>().ReverseMap();

            CreateMap<FeedbacksModel, FeedbacksDTO>().ReverseMap();

            /**********************************************/

            CreateMap<PostImageModel, PostImageDTO>().ReverseMap();

            CreateMap<FolderModel, FolderDTO>()
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children))
                .ReverseMap();

            /**********************************************/
            CreateMap<PostPdfModel, PostPdfDTO>().ReverseMap();

            CreateMap<FolderPdfModel, FolderPdfDTO>()
                .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children))
                .ReverseMap();

            /**********************************************/
            CreateMap<WebsiteSettingsModel, WebsiteSettingsDTO>().ReverseMap();

        }

    }
}

