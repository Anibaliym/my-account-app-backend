using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Vignette;

namespace MyAccountApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VignetteController : ControllerBase
    {
        private readonly IVignetteAppService _vignetteAppService;

        public VignetteController(IVignetteAppService vignetteAppService)
        {
            _vignetteAppService = vignetteAppService;
        }

        [HttpGet("GetVignetteById/{id:guid}")]
        public async Task<VignetteViewModel> GetVignetteById(Guid id)
        {
            return await _vignetteAppService.GetVignetteById(id);
        }

        [HttpGet("GetVignetteByCardId/{cardId:guid}")]
        public async Task<IEnumerable<VignetteViewModel>> GetVignetteByCardId(Guid cardId)
        {
            return await _vignetteAppService.GetVignetteByCardId(cardId);
        }

        [HttpPost("CreateVignette")]
        public async Task<IActionResult> CreateVignette(VignetteCreateViewModel model)
        {
            try
            {
                GenericResponse response = await _vignetteAppService.CreateVignette(model);
                if (response.Resolution)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Se produjo un error al procesar su solicitud. Detalles: {error.Message}");
            }
        }

        [HttpPut("UpdateVignette")]
        public async Task<IActionResult> UpdateVignette(VignetteViewModel model)
        {
            try
            {
                GenericResponse response = await _vignetteAppService.UpdateVignette(model);

                if (response.Resolution)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Se produjo un error al procesar su solicitud. Detalles: {error.Message}");
            }
        }

        [HttpDelete("DeleteVignette")]
        public async Task<IActionResult> DeleteVignette(Guid id)
        {
            try
            {
                GenericResponse response = await _vignetteAppService.DeleteVignette(id);

                if (response.Resolution)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Se produjo un error al procesar su solicitud. Detalles: {error.Message}");
            }
        }
    }
}
