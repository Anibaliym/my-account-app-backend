using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.User;

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
                GenericResponse response = await _domainServices.Login(loginModel.Email, loginModel.Password);

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
    }
}

