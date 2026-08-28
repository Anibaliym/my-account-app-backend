using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Application.Constants;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Vignette;

namespace MyAccountApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VignetteController : StandardGetControllerBase
    {
        private readonly IVignetteAppService _vignetteAppService;

        public VignetteController(IVignetteAppService vignetteAppService)
        {
            _vignetteAppService = vignetteAppService;
        }

        [HttpGet("GetVignetteById/{id:guid}")]
        [ProducesResponseType(typeof(GenericResponse<VignetteViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<VignetteViewModel>), StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetVignetteById(Guid id)
        {
            return GetSingle(id, () => _vignetteAppService.GetVignetteById(id), ErrorCodes.Vignette.NotFound);
        }

        [HttpGet("GetVignetteByCardId/{cardId:guid}")]
        [ProducesResponseType(typeof(GenericResponse<List<VignetteViewModel>>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetVignetteByCardId(Guid cardId)
        {
            return GetCollection(cardId, () => _vignetteAppService.GetVignetteByCardId(cardId));
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
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
            }
        }

        [HttpPut("UpdateVignetteOrderItems")]
        public async Task<IActionResult> UpdateVignetteOrderItems(List<UpdateVignetteViewModel> model)
        {
            try
            {
                GenericResponse response = await _vignetteAppService.UpdateVignetteOrderItems(model);

                if (response.Resolution)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
            }
        }
    }
}
