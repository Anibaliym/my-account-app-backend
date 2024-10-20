using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.User;
using MyAccountApp.Core.Entities;

namespace MyAccountApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _userAppService;

        public UserController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginViewModel loginModel)
        {
            GenericResponse response = await _userAppService.Login(loginModel.Email, loginModel.Password);

            if (!response.Resolution)
            {
                return Unauthorized(new
                {
                    message = response.Message,
                    errors = response.Errors
                });
            }

            // Aquí puedes devolver el token JWT o cualquier otro dato necesario si el login fue exitoso
            return Ok(new
            {
                message = "Inicio de sesión exitoso.",
                user = response.Data // Aquí puedes incluir el objeto de usuario
            });
        }



        [HttpGet("GetActiveUserById/{id:guid}")]
        public async Task<UserViewModel> GetActiveUserById(Guid id)
        {
            return await _userAppService.GetActiveUserById(id);
        }

        [HttpGet("GetAllActiveUsers")]
        public async Task<IEnumerable<UserViewModel>> GetAllActiveUsers()
        {
            return await _userAppService.GetAllActiveUsers();
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(UserCreateViewModel modelo)
        {
            try
            {
                GenericResponse response = await _userAppService.RegisterUser(modelo);

                if (response.Resolution)
                    return CreatedAtAction(nameof(GetActiveUserById), new { id = ((User)response.Data).Id }, response);
                else
                    return BadRequest(response);
            }
            catch (Exception error)
            {
                return StatusCode(500, $"Se produjo un error al procesar su solicitud. Detalles: {error.Message}");
            }
        }

        [HttpPut("UpdateUser/{id:guid}")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateViewModel modelo)
        {
            try
            {
                GenericResponse response = await _userAppService.UpdateUser(modelo);

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

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                GenericResponse response = await _userAppService.DeleteUser(id);

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
