using MyAccountApp.Core.Entities;

namespace MyAccountApp.Core.Interfaces
{
    public interface IVignetteRepository
    {
        Task<Vignette> GetVignetteById(Guid id);
        Task<IEnumerable<Vignette>> GetVignetteByCardId(Guid cardId);
        Task CreateVignette(Vignette model);
        Task UpdateVignette(Vignette model);
        Task<bool> DeleteVignette(Guid id);
    }
}
