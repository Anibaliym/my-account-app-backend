using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Sheet;

namespace MyAccountApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SheetController : ControllerBase
    {
        private readonly ISheetAppService _sheetAppService;

        public SheetController(ISheetAppService sheetAppService)
        {
            _sheetAppService = sheetAppService;
        }

        [HttpGet("GetSheetById/{id:guid}")]
        public async Task<SheetViewModel> GetSheetById(Guid id)
        {
            return await _sheetAppService.GetSheetById(id);
        }

        [HttpGet("GetSheetAccountByOrder")]
        public async Task<SheetViewModel> GetSheetAccountByOrder(int order, Guid accountid)
        {
            return await _sheetAppService.GetSheetAccountByOrder(order, accountid);
        }

        [HttpGet("GetSheetByAccountId/{accountId:guid}")]
        public async Task<IEnumerable<SheetViewModel>> GetSheetByAccountId(Guid accountId)
        {
            return await _sheetAppService.GetSheetByAccountId(accountId);
        }

        [HttpPost("CreateSheet")]
        public async Task<IActionResult> CreateSheet(CreateSheetViewModel model)
        {
            try
            {
                GenericResponse response = await _sheetAppService.CreateSheet(model);

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

        [HttpPut("UpdateSheet")]
        public async Task<IActionResult> UpdateSheet(UpdateSheetViewModel model)
        {
            try
            {
                GenericResponse response = await _sheetAppService.UpdateSheet(model);

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

        [HttpPatch("UpdateCashBalance/{sheetId:guid}")]
        public async Task<IActionResult> UpdateCashBalance(Guid sheetId, [FromBody] int newCashBalance)
        {
            try
            {
                GenericResponse response = await _sheetAppService.UpdateCashBalance(sheetId, newCashBalance);

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

        [HttpPatch("UpdateCurrenteAccountBalance/{sheetId:guid}")]
        public async Task<IActionResult> UpdateCurrenteAccountBalance(Guid sheetId, [FromBody] int newCashBalance)
        {
            try
            {
                GenericResponse response = await _sheetAppService.UpdateCurrenteAccountBalance(sheetId, newCashBalance);

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


        [HttpPut("UpdateSheetOrderItems")]
        public async Task<IActionResult> UpdateSheetOrderItems(List<UpdateSheetViewModel> model)
        {
            try
            {
                GenericResponse response = await _sheetAppService.UpdateSheetOrderItems(model);

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


        [HttpDelete("DeleteSheet")]
        public async Task<IActionResult> DeleteSheet(Guid id)
        {
            try
            {
                GenericResponse response = await _sheetAppService.DeleteSheet(id);

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
