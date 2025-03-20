using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Account;

namespace MyAccountApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountAppService _accountAppService;

        public AccountController(IAccountAppService accountAppService)
        {
            _accountAppService = accountAppService;
        }

        [HttpGet("GetAccountById/{id:guid}")]
        public async Task<AccountViewModel> GetAccountById(Guid id)
        {
            return await _accountAppService.GetAccountById(id);
        }

        [HttpGet("GetAccountByUserId/{userId:guid}")]
        public async Task<IEnumerable<AccountViewModel>> GetAccountByUserId(Guid userId)
        {
            return await _accountAppService.GetAccountByUserId(userId);
        }

        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel model)
        {
            try
            {
                GenericResponse response = await _accountAppService.CreateAccount(model);

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

        [HttpPut("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount(UpdateAccountViewModel model)
        {
            try
            {
                GenericResponse response = await _accountAppService.UpdateAccount(model);

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

        [HttpPut("UpdateAccountOrderItems")]
        public async Task<IActionResult> UpdateAccountOrderItems(List<UpdateAccountViewModel> model)
        {
            try
            {
                GenericResponse response = await _accountAppService.UpdateAccountOrderItems(model);

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

        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            try
            {
                GenericResponse response = await _accountAppService.DeleteAccount(id);

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
