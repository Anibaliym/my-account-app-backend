using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Application;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.User;
using MyAccountApp.Application.ViewModels.Vignette;

namespace MyAccountApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DomainServicesController : ControllerBase
    {
        private readonly IDomainServicesAppService _domainServices;

        public DomainServicesController(IDomainServicesAppService domainServices){
            _domainServices = domainServices;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginViewModel loginModel)
        {
            try
            {
                string? ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                
                string? userAgent = Request.Headers["User-Agent"].ToString();

                GenericResponse response = await _domainServices.Login(loginModel.Email, loginModel.Password, ip, userAgent);

                if (response.Resolution)
                    return Ok(response);
                else
                    return Unauthorized(response);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Se produjo un error al procesar su solicitud. Detalles: {error.Message}");
            }
        }

        [HttpGet("GetSheetsAccount/{accountId:guid}")]
        public async Task<GenericResponse> GetSheetsAccount(Guid accountId)
        {
            return await _domainServices.GetSheetsAccount(accountId);
        }

        [HttpGet("GetUserAccountsWithSheets/{userId:guid}")]
        public async Task<GenericResponse> GetUserAccountsWithSheets(Guid userId)
        {
            return await _domainServices.GetUserAccountsWithSheets(userId);
        }

        [HttpGet("GetSheetCardsWithVignettes/{sheetId:guid}")]
        public async Task<GenericResponse> GetSheetCardsWithVignettes(Guid sheetId)
        {
            return await _domainServices.GetSheetCardsWithVignettes(sheetId);
        }

        [HttpDelete("DeleteCardWithVignettes/{cardId:guid}")]
        public async Task<IActionResult> DeleteCardWithVignettes(Guid cardId)
        {
            try
            {
                GenericResponse response = await _domainServices.DeleteCardWithVignettes(cardId);

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

        [HttpDelete("DeleteUserAccount")]
        public async Task<IActionResult> DeleteUserAccount(DeleteUserRequest request)
        {
            try
            {
                GenericResponse response = await _domainServices.DeleteUserAccount(request);

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

        [HttpPut("UpdateVignetteAndRecalculateTotal")]
        public async Task<IActionResult> UpdateVignetteAndRecalculateTotal(VignetteViewModel model)
        {
            try
            {
                GenericResponse response = await _domainServices.UpdateVignetteAndRecalculateTotal(model);

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

        [HttpDelete("DeleteVignetteAndRecalculateTotal")]
        public async Task<IActionResult> DeleteVignetteAndRecalculateTotal(Guid vignetteId)
        {
            try
            {
                GenericResponse response = await _domainServices.DeleteVignetteAndRecalculateTotal(vignetteId);

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

        [HttpPut("UpdateVignetteColorTheme")]
        public async Task<IActionResult> UpdateVignetteColorTheme(Guid vignetteId, string colorTheme)
        {
            try
            {
                GenericResponse response = await _domainServices.UpdateVignetteColorTheme(vignetteId, colorTheme);

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

        [HttpPost("CreateSheetBackup")]
        public async Task<IActionResult> CreateSheetBackup(Guid sheetId)
        {
            try
            {
                GenericResponse response = await _domainServices.CreateSheetBackup(sheetId);

                if (response.Resolution)
                    return Ok(response);
                else
                    return Unauthorized(response);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Se produjo un error al procesar su solicitud. Detalles: {error.Message}");
            }
        }

        [HttpDelete("DeleteSheetWithContents")]
        public async Task<IActionResult> DeleteSheetWithContents(Guid sheetId)
        {
            try
            {
                GenericResponse response = await _domainServices.DeleteSheetWithContents(sheetId);

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

