using AutoMapper;
using MyAccountApp.Application.ViewModels.Card;
using MyAccountApp.Application.ViewModels.Account;
using MyAccountApp.Application.ViewModels.Sheet;
using MyAccountApp.Application.ViewModels.User;
using MyAccountApp.Application.ViewModels.Vignette;
using MyAccountApp.Core.Entities;

namespace MyAccountApp.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<UserViewModel, User>();
            CreateMap<UserCreateViewModel, User>();
            CreateMap<UserUpdateViewModel, User>();

            CreateMap<AccountViewModel, Account>();
            CreateMap<CreateAccountViewModel, Account>();
            CreateMap<UpdateAccountViewModel, Account>();

            CreateMap<SheetViewModel, Sheet>();
            CreateMap<CreateSheetViewModel, Sheet>();
            CreateMap<UpdateSheetViewModel, Sheet>();

            CreateMap<CardViewModel, Card>();
            CreateMap<CreateCardViewModel, Card>();
            CreateMap<UpdateCardViewModel, Card>();

            CreateMap<VignetteViewModel, Vignette>();
            CreateMap<VignetteCreateViewModel, Vignette>();
        }
    }
}
