using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Application;
using MyAccountApp.Application.Constants;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.User;
using MyAccountApp.Application.ViewModels.Vignette;

namespace MyAccountApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DomainServicesController : StandardGetControllerBase
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
            catch (Exception)
            {
                return UnexpectedFailure();
            }
        }

        [HttpGet("GetSheetsAccount/{accountId:guid}")]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetSheetsAccount(Guid accountId)
        {
            return GetStandardResponse(accountId, () => _domainServices.GetSheetsAccount(accountId), ErrorCodes.Account.NotFound);
        }

        [HttpGet("GetUserAccountsWithSheets/{userId:guid}")]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetUserAccountsWithSheets(Guid userId)
        {
            return GetStandardResponse(userId, () => _domainServices.GetUserAccountsWithSheets(userId), ErrorCodes.User.NotFound);
        }

        [HttpGet("GetSheetCardsWithVignettes/{sheetId:guid}")]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetSheetCardsWithVignettes(Guid sheetId)
        {
            return GetStandardResponse(sheetId, () => _domainServices.GetSheetCardsWithVignettes(sheetId), ErrorCodes.Sheet.NotFound);
        }

        [HttpGet("GetAllSuccessUserAccessLogByUserId/{userId:guid}")]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetAllSuccessUserAccessLogByUserId(Guid userId)
        {
            return GetStandardResponse(userId, () => _domainServices.GetAllSuccessUserAccessLogByUserId(userId), ErrorCodes.User.NotFound);
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
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
            }
        }
    }
}

