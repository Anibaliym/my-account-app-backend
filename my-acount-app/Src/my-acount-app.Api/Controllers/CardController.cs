using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Card;

namespace MyAccountApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ICardAppService _cardAppService;

        public CardController(ICardAppService cardAppService)
        {
            _cardAppService = cardAppService;
        }

        [HttpGet("GetCardById/{id:guid}")]
        public async Task<CardViewModel> GetCardById(Guid id)
        {
            return await _cardAppService.GetCardById(id);
        }

        [HttpGet("GetCardBySheetId/{sheetId:guid}")]
        public async Task<IEnumerable<CardViewModel>> GetCardBySheetId(Guid sheetId)
        {
            return await _cardAppService.GetCardBySheetId(sheetId);
        }

        [HttpPost("CreateCard")]
        public async Task<IActionResult> CreateCard(CreateCardViewModel model)
        {
            try 
            { 
                GenericResponse response = await _cardAppService.CreateCard(model);
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

        [HttpPut("UpdateCard")]
        public async Task<IActionResult> UpdateCard(UpdateCardViewModel model)
        {
            try
            {
                GenericResponse response = await _cardAppService.UpdateCard(model);

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

        [HttpDelete("DeleteCard")]
        public async Task<IActionResult> DeleteCard([FromHeader] Guid id)
        {
            try
            {
                GenericResponse response = await _cardAppService.DeleteCard(id);

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
