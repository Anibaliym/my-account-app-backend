using AutoMapper;
using MyAccountApp.Application.ViewModels.Card;
using MyAccountApp.Application.ViewModels.Account;
using MyAccountApp.Application.ViewModels.User;
using MyAccountApp.Application.ViewModels.Vignette;
using MyAccountApp.Core.Entities;
using MyAccountApp.Application.ViewModels.Sheet;

namespace MyAccountApp.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<Account, AccountViewModel>();
            CreateMap<Sheet, SheetViewModel>();
            CreateMap<Card, CardViewModel>();
            CreateMap<Vignette, VignetteViewModel>();
        }
    }
}
