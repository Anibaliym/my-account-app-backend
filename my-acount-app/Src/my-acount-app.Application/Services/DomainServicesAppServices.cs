using MyAccountApp.Application.Interfaces;
using FluentValidation;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.UserSecurity;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Enum.User;
using MyAccountApp.Core.Interfaces;
using MyAccountApp.Core.Utils;

namespace MyAccountApp.Application.Services
{
    public class DomainServicesAppServices : IDomainServicesAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserSecurityRepository _userSecurityRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ISheetRepository _sheetRepository;

        public DomainServicesAppServices(
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            ISheetRepository sheetRepository, 
            IUserSecurityRepository userSecurityRepository,
            IValidator<UserSecurityCreateViewModel> createUserSecurityValidator

        )
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _sheetRepository = sheetRepository;
            _userSecurityRepository = userSecurityRepository; 
        }

        public async Task<GenericResponse> Login(string email, string password)
        {
            User userFound = await _userRepository.GetActiveUserByEmail(email.ToUpper());
            LoginResponseViewModel responseModel = new LoginResponseViewModel(); 
            
            if (userFound == null)
            {
                return new GenericResponse
                {
                    Resolution = false,
                    Errors = new[] { "Usuario o contraseña incorrectos." },
                    Message = "Se encontraron errores de validación."
                };
            }

            if (userFound.RegistrationMethod == UserRegistrationMethodEnum.GOOGLE_AUTH.Name)
            {
                return new GenericResponse
                {
                    Resolution = false,
                    Errors = new[] { $"El usuario con el correo '{ email }', se ha registrado con la autenticación de google y no con la autenticación propia del sistema." },
                    Message = "Se encontraron errores de validación."
                };
            }

            //Se obtiene la información de la seguridad del usuario.
            UserSecurity userSecurityFound = await _userSecurityRepository.GetUserSecurityByUserId(userFound.Id);

            if (userSecurityFound == null)
            {
                return new GenericResponse
                {
                    Resolution = false,
                    Errors = new[] { "Usuario o contraseña incorrectos." },
                    Message = "Se encontraron errores de validación."
                };
            }

            bool isPasswordValid = PasswordUtils.VerifyPasswordHash(password, Convert.FromBase64String(userSecurityFound.PasswordHash), Convert.FromBase64String(userSecurityFound.PasswordSalt));

            if (!isPasswordValid)
            {
                return new GenericResponse
                {
                    Resolution = false,
                    Errors = new[] { "Usuario o contraseña incorrectos." },
                    Message = "Se encontraron errores de validación."
                };
            }
            
            responseModel.User = userFound; 
            responseModel.Accounts = new List<AccountDto>();

            // Se obtienen todas las cuentas con sus respectivas hojas de cálculo del usuario. 
            IEnumerable<Account> userAccounts = await _accountRepository.GetActiveAccountByUserId(userFound.Id);

            foreach (Account account in userAccounts)
            {
                AccountDto accountDto = new AccountDto { 
                    Account = account, 
                    Sheets = new List<SheetDto>() 
                };

                // Obtenemos todas las hojas asociadas a la cuenta
                IEnumerable<Sheet> sheets = await _sheetRepository.GetSheetByAccountId(account.Id);

                // Se agregan las hojas a la cuenta
                foreach (Sheet sheet in sheets)
                    accountDto.Sheets.Add(new SheetDto { Sheet = sheet });

                // Se agrega la cuenta con sus hojas al responseModel
                responseModel.Accounts.Add(accountDto);
            }

            // Si todo es correcto, devolver el usuario encontrado con las cuentas y hojas
            return new GenericResponse
            {
                Resolution = true,
                Data = responseModel, 
                Message = "Inicio de sesión exitoso."
            };
        }

        // public async Task<GenericResponse> GetSheetsAccount(Guid accountId)
        // {
        //     IEnumerable<Sheet> sheets; 
        //     Account account = await _accountRepository.GetActiveAccountById(accountId);

        //     if(account != null){
        //         sheets = await _sheetRepository.GetSheetByAccountId(accountId);

        //         if(sheets.Count() > 0){
        //             return new GenericResponse {
        //                 Resolution = true,
        //                 Data = new { 
        //                     Account = new {
        //                         name = account.Description,
        //                         creationDate = account.CreationDate,
        //                     }, 
        //                     Sheets = sheets,
        //                 },
        //             }; 
        //         }
        //         else {
        //             return new GenericResponse {
        //                 Resolution = true,
        //                 Message = $"La cuenta con el id { accountId }, no tiene hojas de calculo."
        //             }; 
        //         }
        //     }
        //     else {
        //         return new GenericResponse {
        //             Resolution = false,
        //             Message = $"La cuenta con el id '{ accountId }', no existe."
        //         }; 
        //     }
        // }


        public async Task<GenericResponse> GetSheetsAccount(Guid accountId)
        {
            IEnumerable<Sheet> sheets; 
            Account account = await _accountRepository.GetActiveAccountById(accountId);

            if(account != null){
                sheets = await _sheetRepository.GetSheetByAccountId(accountId);

                return new GenericResponse {
                    Resolution = true,
                    Data = new { 
                        Account = new {
                            name = account.Description,
                            creationDate = account.CreationDate,
                        }, 
                        Sheets = sheets,
                    },
                }; 

            }
            else {
                return new GenericResponse {
                    Resolution = false,
                    Message = $"La cuenta con el id '{ accountId }', no existe."
                }; 
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

