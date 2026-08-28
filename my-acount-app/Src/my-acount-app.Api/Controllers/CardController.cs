using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Application.Constants;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Card;

namespace MyAccountApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : StandardGetControllerBase
    {
        private readonly ICardAppService _cardAppService;

        public CardController(ICardAppService cardAppService)
        {
            _cardAppService = cardAppService;
        }

        [HttpGet("GetCardById/{id:guid}")]
        [ProducesResponseType(typeof(GenericResponse<CardViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<CardViewModel>), StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetCardById(Guid id)
        {
            return GetSingle(id, () => _cardAppService.GetCardById(id), ErrorCodes.Card.NotFound);
        }

        [HttpGet("GetCardBySheetId/{sheetId:guid}")]
        [ProducesResponseType(typeof(GenericResponse<List<CardViewModel>>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetCardBySheetId(Guid sheetId)
        {
            return GetCollection(sheetId, () => _cardAppService.GetCardBySheetId(sheetId));
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

        [HttpPut("UpdateCardOrderItems")]
        public async Task<IActionResult> UpdateCardOrderItems(List<UpdateCardViewModel> model)
        {
            try
            {
                GenericResponse response = await _cardAppService.UpdateCardOrderItems(model);

                if (response.Resolution)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Se produjo un error al procesar su solicitud. Detalles: { error.Message }");
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
