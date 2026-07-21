using AutoMapper;
using FluentValidation;
using MyAccountApp.Application.Constants;
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
            UserSecurity userSecurity = new UserSecurity(); 
            User user = _mapper.Map<User>(model);

            FluentValidation.Results.ValidationResult validationResult = await _createUserValidator.ValidateAsync(model);

            if (!validationResult.IsValid) {
                return new GenericResponse {
                    Resolution = false,
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                    ErrorCode = ErrorCodes.Common.ValidationFailed,
                    Message = ResponseMessages.Common.ValidationFailed, 
                };
            }

            FluentValidation.Results.ValidationResult validationUserSecurityResult = await _createUserSecurityValidator.ValidateAsync(model.UserSecurity);

            if (!validationUserSecurityResult.IsValid)
            {
                if (model.RegistrationMethod == UserRegistrationMethodEnum.MANUAL_AUTH.Name) { 
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ValidationFailed, 
                        Errors = validationUserSecurityResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                        Message = ResponseMessages.Common.ValidationFailed, 
                    };
                }
            }

            try
            {
                User userExistsByEmail = await _userRepository.GetUserByEmail(model.Email.ToUpper());

                if (userExistsByEmail != null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ValidationFailed, 
                        Errors = [ $"The email '{model.Email.ToUpper()}' is already registered."] ,
                        Message = ResponseMessages.Common.UnexpectedException,
                    }; 
                }

                user.Id = Guid.NewGuid();
                user.CreationDate = DateTime.UtcNow;
                user.FirstName = user.FirstName.ToUpper();
                user.LastName = user.LastName.ToUpper();
                user.Email = user.Email.ToUpper();

                await _userRepository.CreateUser(user);

                //Crea la seguridad del usuario en el caso que el usuario haya elegido la autenticación propia del sistema.
                if (model.RegistrationMethod == UserRegistrationMethodEnum.MANUAL_AUTH.Name)
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

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Common.Created, 
                    Data = user,
                }; 
            }
            catch (Exception ex)
            {
                return new GenericResponse {
                    Resolution = false,
                    Message = ResponseMessages.Common.UnexpectedError, 
                    ErrorCode = ErrorCodes.Common.UnexpectedError
                }; 
            }
        }

        public async Task<GenericResponse> UpdateUser(UserUpdateViewModel model)
        {
            FluentValidation.Results.ValidationResult validationResult = await _updateUserValidator.ValidateAsync(model);

            if (!validationResult.IsValid) {
                return new GenericResponse {
                    Resolution = false,
                    ErrorCode = ErrorCodes.Common.ValidationFailed, 
                    Message = ResponseMessages.Common.ValidationFailed, 
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                };
            }

            try
            {
                User existingUser = await _userRepository.GetUserById(model.Id);
                
                if (existingUser == null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound, 
                        Message = ResponseMessages.User.NotFound(model.Id), 
                        Errors = [ResponseMessages.User.NotFound(model.Id)], 
                    };
                }

                // Mapear solo las propiedades necesarias desde el modelo
                _mapper.Map(model, existingUser);

                // Asegúrate de que FechaCreacion está en UTC
                existingUser.CreationDate = existingUser.CreationDate.ToUniversalTime();

                await _userRepository.UpdateUser(existingUser);

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Common.Updated, 
                    Data = existingUser,
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse {
                    Resolution = false,
                    Message = ResponseMessages.Common.UnexpectedError, 
                    ErrorCode = ErrorCodes.Common.UnexpectedError
                }; 
            }
        }

        public async Task<GenericResponse> DeleteUser(Guid id)
        {
            try
            {
                User existingUser = await _userRepository.GetUserById(id);

                if (existingUser == null)
                {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ValidationFailed, 
                        Message = ResponseMessages.Common.ValidationFailed,
                        Errors = [ ResponseMessages.User.NotFound(id) ],
                    };
                }

                bool resolution = await _userRepository.DeleteUser(id);

                return new GenericResponse {
                    Resolution = resolution,
                    ErrorCode = resolution ? null : ErrorCodes.Common.OperationFailed,
                    Message = resolution ? ResponseMessages.Common.Deleted : ResponseMessages.Common.OperationFailed,
                }; 
            }
            catch (Exception ex)
            {
                return new GenericResponse {
                    Resolution = false,
                    Message = ResponseMessages.Common.UnexpectedError, 
                    ErrorCode = ErrorCodes.Common.UnexpectedError
                }; 
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
