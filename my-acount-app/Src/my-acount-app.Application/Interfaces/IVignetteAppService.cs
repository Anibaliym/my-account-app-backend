using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Vignette;

namespace MyAccountApp.Application.Interfaces
{
    public interface IVignetteAppService : IDisposable
    {
        Task<VignetteViewModel> GetVignetteById(Guid id);
        Task<IEnumerable<VignetteViewModel>> GetVignetteByCardId(Guid cardId); 
        Task<GenericResponse> CreateVignette(VignetteCreateViewModel model);
        Task<GenericResponse> UpdateVignette(VignetteViewModel model);
        Task<GenericResponse> UpdateVignetteOrderItems(List<UpdateVignetteViewModel> model);
        Task<GenericResponse> DeleteVignette(Guid id);
    }
}
