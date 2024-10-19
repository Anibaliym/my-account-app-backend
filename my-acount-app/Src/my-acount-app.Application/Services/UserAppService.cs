using AutoMapper;
using FluentValidation;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.User;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Interfaces;

namespace MyAccountApp.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<UserCreateViewModel> _createUserValidator;
        private readonly IValidator<UserUpdateViewModel> _updateUserValidator;
        private readonly IMapper _mapper;

        public UserAppService(
            IMapper mapper,
            IUserRepository userRepository, 
            IValidator<UserCreateViewModel> createUserValidator, 
            IValidator<UserUpdateViewModel> updateUserValidator
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
        }

        public async Task<UserViewModel> GetActiveUserById(Guid id)
        {
            return _mapper.Map<UserViewModel>(await _userRepository.GetActiveUserById(id));
        }

        public async Task<UserViewModel> GetActiveUserByEmail(string email)
        {
            return _mapper.Map<UserViewModel>(await _userRepository.GetActiveUserByEmail(email));
        }

        public async Task<IEnumerable<UserViewModel>> GetAllActiveUsers()
        {
            return _mapper.Map<IEnumerable<UserViewModel>>(await _userRepository.GetAllActiveUsers());
        }

        public async Task<GenericResponse> RegisterUser(UserCreateViewModel model)
        {
            GenericResponse response = new GenericResponse();
            User user = _mapper.Map<User>(model);

            FluentValidation.Results.ValidationResult validationResult = _createUserValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                return new GenericResponse
                {
                    Resolution = false,
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                    Message = "Se encontraron errores de validación."
                };
            }

            try
            {
                User userExistsByEmail = await _userRepository.GetActiveUserByEmail(model.Email.ToUpper());


                if (userExistsByEmail != null)
                {
                    response.Resolution = false;
                    response.Message = $"El usuario con el correo '{model.Email.ToUpper()}', ya existe.";
                    return response;
                }

                user.Id = Guid.NewGuid();
                user.CreationDate = DateTime.UtcNow;
                user.IsActive = true;
                user.FirstName = user.FirstName.ToUpper();
                user.LastName = user.LastName.ToUpper();
                user.Email = user.Email.ToUpper();

                await _userRepository.CreateUser(user);
                response.Resolution = true;
                response.Data = user;
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<GenericResponse> UpdateUser(UserUpdateViewModel model)
        {
            GenericResponse response = new GenericResponse();

            FluentValidation.Results.ValidationResult validationResult = _updateUserValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                return new GenericResponse
                {
                    Resolution = false,
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                    Message = "Se encontraron errores de validación."
                };
            }

            try
            {
                User existingUser = await _userRepository.GetActiveUserById(model.Id);
                
                if (existingUser == null) {
                    response.Resolution = false;
                    response.Message = $"Usuario con el id '{model.Id }', no existe.";
                    return response;
                }

                // Mapear solo las propiedades necesarias desde el modelo
                _mapper.Map(model, existingUser);

                // Asegúrate de que FechaCreacion está en UTC
                existingUser.CreationDate = existingUser.CreationDate.ToUniversalTime();

                await _userRepository.UpdateUser(existingUser);
                response.Resolution = true;
                response.Data = existingUser;
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<GenericResponse> DeleteUser(Guid id)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                User existingUser = await _userRepository.GetActiveUserById(id);

                if (existingUser == null)
                {
                    response.Resolution = false;
                    response.Message = "Se encontraron errores de validación.";
                    response.Errors = [$"El usuario con el id {id}, no existe"];
                    return response;
                }

                bool resolution = await _userRepository.DeleteUser(id);
                response.Resolution = resolution;
                response.Message = (resolution) ? "Usuario eliminado" : "No se pudo eliminar el registro.";
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Message = "Se encontraron errores de validación";
                response.Errors = [ex.Message]; 
            }
            return response;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
