using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Application.Constants;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Sheet;

namespace MyAccountApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SheetController : StandardGetControllerBase
    {
        private readonly ISheetAppService _sheetAppService;

        public SheetController(ISheetAppService sheetAppService)
        {
            _sheetAppService = sheetAppService;
        }

        [HttpGet("GetSheetById/{id:guid}")]
        [ProducesResponseType(typeof(GenericResponse<SheetViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<SheetViewModel>), StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetSheetById(Guid id)
        {
            return GetSingle(id, () => _sheetAppService.GetSheetById(id), ErrorCodes.Sheet.NotFound);
        }

        [HttpGet("GetSheetAccountByOrder")]
        [ProducesResponseType(typeof(GenericResponse<SheetViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<SheetViewModel>), StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetSheetAccountByOrder(int order, Guid accountid)
        {
            if (order < 0)
                return Task.FromResult<IActionResult>(BadRequest(new GenericResponse<SheetViewModel>
                {
                    Resolution = false,
                    Data = null,
                    Message = ResponseMessages.Common.ValidationFailed,
                    ErrorCode = ErrorCodes.Common.ValidationFailed,
                    Errors = new[] { "The order must be zero or greater." }
                }));

            return GetSingle(accountid, () => _sheetAppService.GetSheetAccountByOrder(order, accountid), ErrorCodes.Sheet.NotFound);
        }

        [HttpGet("GetSheetByAccountId/{accountId:guid}")]
        [ProducesResponseType(typeof(GenericResponse<List<SheetViewModel>>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetSheetByAccountId(Guid accountId)
        {
            return GetCollection(accountId, () => _sheetAppService.GetSheetByAccountId(accountId));
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
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
            }
        }

        [HttpPut("UpdateSheetTitle")]
        [ProducesResponseType(typeof(GenericResponse<SheetViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<SheetViewModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(GenericResponse<SheetViewModel>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GenericResponse<SheetViewModel>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSheetTitle([FromQuery] Guid sheetId, [FromQuery] string sheetNewTitle)
        {
            try
            {
                GenericResponse<SheetViewModel> response = await _sheetAppService.UpdateSheetTitle(sheetId, sheetNewTitle);

                if (response.Resolution)
                    return Ok(response);

                if (response.ErrorCode == ErrorCodes.Sheet.NotFound)
                    return NotFound(response);

                if (response.ErrorCode == ErrorCodes.Common.UnexpectedError)
                    return StatusCode(
                        StatusCodes.Status500InternalServerError,
                        response
                    );

                return BadRequest(response);
            }
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
            }
        }
    }
}
