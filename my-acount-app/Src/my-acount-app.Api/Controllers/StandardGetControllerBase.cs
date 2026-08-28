using Microsoft.AspNetCore.Mvc;
using MyAccountApp.Application.Responses;

namespace MyAccountApp.Api.Controllers
{
    public abstract class StandardGetControllerBase : ControllerBase
    {
        protected async Task<IActionResult> GetSingle<T>(Guid id, Func<Task<T>> query, string notFoundErrorCode) where T : class
        {
            if (id == Guid.Empty)
                return ValidationFailure<T>("The identifier must not be empty.");

            try
            {
                T? data = await query();
                
                if (data == null)
                {
                    return NotFound(new GenericResponse<T>
                    {
                        Resolution = false,
                        Data = null,
                        Message = "The requested resource was not found.",
                        ErrorCode = notFoundErrorCode,
                        Errors = null
                    });
                }

                return Ok(Success(data));
            }
            catch (Exception)
            {
                return UnexpectedFailure<T>();
            }
        }

        protected async Task<IActionResult> GetCollection<T>(Guid? id, Func<Task<IEnumerable<T>>> query)
        {
            if (id == Guid.Empty)
                return ValidationFailure<List<T>>("The identifier must not be empty.");

            try
            {
                List<T> data = (await query() ?? Enumerable.Empty<T>()).ToList();
                
                return Ok(Success(data));
            }
            catch (Exception)
            {
                return UnexpectedFailure<List<T>>();
            }
        }

        protected async Task<IActionResult> GetStandardResponse(Guid id, Func<Task<GenericResponse>> query, string failureErrorCode)
        {
            if (id == Guid.Empty)
                return ValidationFailure<object>("The identifier must not be empty.");

            try
            {
                GenericResponse response = await query();
                var typedResponse = new GenericResponse<object>
                {
                    Resolution = response.Resolution,
                    Data = response.Resolution ? response.Data : null,
                    Message = response.Resolution ? ResponseMessages.Success : response.Message,
                    ErrorCode = response.Resolution ? null : failureErrorCode,
                    Errors = response.Errors
                };

                return response.Resolution ? Ok(typedResponse) : NotFound(typedResponse);
            }
            catch (Exception)
            {
                return UnexpectedFailure<object>();
            }
        }

        protected static GenericResponse<T> Success<T>(T data) => new()
        {
            Resolution = true,
            Data = data,
            Message = ResponseMessages.Success,
            ErrorCode = null,
            Errors = null
        };

        private BadRequestObjectResult ValidationFailure<T>(string error) => BadRequest(new GenericResponse<T>
        {
            Resolution = false,
            Data = default,
            Message = ResponseMessages.ValidationFailed,
            ErrorCode = ErrorCodes.CommonValidationFailed,
            Errors = new[] { error }
        });

        private ObjectResult UnexpectedFailure<T>() => StatusCode(StatusCodes.Status500InternalServerError,
            new GenericResponse<T>
            {
                Resolution = false,
                Data = default,
                Message = ResponseMessages.UnexpectedError,
                ErrorCode = ErrorCodes.CommonUnexpectedError,
                Errors = null
            });
    }
}
