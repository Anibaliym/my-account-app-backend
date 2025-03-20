using AutoMapper;
using FluentValidation;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.User;
using MyAccountApp.Application.ViewModels.UserSecurity;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Enum.User;
using MyAccountApp.Core.Interfaces;
using MyAccountApp.Core.Utils;

namespace MyAccountApp.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserSecurityRepository _userSecurityRepository;
        private readonly IValidator<UserCreateViewModel> _createUserValidator;
        private readonly IValidator<UserSecurityCreateViewModel> _createUserSecurityValidator;
        private readonly IValidator<UserUpdateViewModel> _updateUserValidator;
        private readonly IMapper _mapper;

        public UserAppService(
            IMapper mapper,
            IUserRepository userRepository,
            IUserSecurityRepository userSecurityRepository,
            IValidator<UserCreateViewModel> createUserValidator, 
            IValidator<UserUpdateViewModel> updateUserValidator,
            IValidator<UserSecurityCreateViewModel> createUserSecurityValidator
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _userSecurityRepository = userSecurityRepository; 
            _createUserValidator = createUserValidator;
            _updateUserValidator = updateUserValidator;
            _createUserSecurityValidator = createUserSecurityValidator;
        }

        public async Task<UserViewModel> GetUserById(Guid id)
        {
            return _mapper.Map<UserViewModel>(await _userRepository.GetUserById(id));
        }

        public async Task<UserViewModel> GetUserByEmail(string email)
        {
            return _mapper.Map<UserViewModel>(await _userRepository.GetUserByEmail(email));
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsers()
        {
            return _mapper.Map<IEnumerable<UserViewModel>>(await _userRepository.GetAllUsers());
        }

        public async Task<GenericResponse> RegisterUser(UserCreateViewModel model)
        {
            GenericResponse response = new GenericResponse();
            UserSecurity userSecurity = new UserSecurity(); 
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

            FluentValidation.Results.ValidationResult validationUserSecurityResult = _createUserSecurityValidator.Validate(model.UserSecurity);

            if (!validationUserSecurityResult.IsValid)
            {
                if (model.RegistrationMethod == UserRegistrationMethodEnum.MANUAL_AUTH.Name) { 
                    return new GenericResponse
                    {
                        Resolution = false,
                        Errors = validationUserSecurityResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                        Message = "Se encontraron errores de validación."
                    };
                }
            }

            try
            {
                User userExistsByEmail = await _userRepository.GetUserByEmail(model.Email.ToUpper());

                if (userExistsByEmail != null)
                {
                    response.Resolution = false;
                    response.Errors = [$"El usuario con el correo '{model.Email.ToUpper()}', ya existe."];
                    response.Message = "Se encontraron errores de validación."; 
                    return response;
                }

                user.Id = Guid.NewGuid();
                user.CreationDate = DateTime.UtcNow;
                user.FirstName = user.FirstName.ToUpper();
                user.LastName = user.LastName.ToUpper();
                user.Email = user.Email.ToUpper();

                await _userRepository.CreateUser(user);
                response.Resolution = true;
                response.Data = user;

                //Crea la seguridad del usuario en el caso que el usuario haya elegido la autenticación propia del sistema.
                if (response.Resolution == true && model.RegistrationMethod == UserRegistrationMethodEnum.MANUAL_AUTH.Name)
                {
                    byte[] passwordHash, passwordSalt;
                    PasswordUtils.CreatePasswordHash(model.UserSecurity.Password, out passwordHash, out passwordSalt);


                    userSecurity.Id = Guid.NewGuid();
                    userSecurity.UserId = user.Id;
                    userSecurity.PasswordHash = Convert.ToBase64String(passwordHash);
                    userSecurity.PasswordSalt = Convert.ToBase64String(passwordSalt);

                    userSecurity.LastPasswordChangeDate = DateTime.UtcNow;

                    await _userSecurityRepository.CreateUserSecurity(userSecurity); 

                }
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
                User existingUser = await _userRepository.GetUserById(model.Id);
                
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
                User existingUser = await _userRepository.GetUserById(id);

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
