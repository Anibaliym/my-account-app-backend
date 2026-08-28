using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Application.Constants;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.User;
using MyAccountApp.Core.Entities;

namespace MyAccountApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : StandardGetControllerBase
    {
        private readonly IUserAppService _userAppService;

        public UserController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpGet("GetUserById/{id:guid}")]
        [ProducesResponseType(typeof(GenericResponse<UserViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<UserViewModel>), StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetUserById(Guid id)
        {
            return GetSingle(id, () => _userAppService.GetUserById(id), ErrorCodes.User.NotFound);
        }

        [HttpGet("GetAllUsers")]
        [ProducesResponseType(typeof(GenericResponse<List<UserViewModel>>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetAllUsers()
        {
            return GetCollection(null, () => _userAppService.GetAllUsers());
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(UserCreateViewModel modelo)
        {
            try
            {
                GenericResponse response = await _userAppService.RegisterUser(modelo);

                if (response.Resolution)
                    return CreatedAtAction(nameof(GetUserById), new { id = ((User)response.Data).Id }, response);
                else
                    return BadRequest(response);
            }
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
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
            catch (Exception)
            {
                return UnexpectedFailure();
            }
        }
    }
}
