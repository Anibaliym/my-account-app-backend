using MyAccountApp.Application.Interfaces;
using FluentValidation;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.UserSecurity;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Enum.User;
using MyAccountApp.Core.Interfaces;
using MyAccountApp.Core.Utils;
using System.Reflection.Metadata.Ecma335;
using MyAccountApp.Application.ViewModels.Vignette;
using AutoMapper;

namespace MyAccountApp.Application.Services
{
    public class DomainServicesAppServices : IDomainServicesAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserSecurityRepository _userSecurityRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ISheetRepository _sheetRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IVignetteRepository _vignetteRepository;
        private readonly IValidator<VignetteViewModel> _updateVignetteValidator;
        private readonly IMapper _mapper;

        public DomainServicesAppServices (
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            ISheetRepository sheetRepository, 
            IUserSecurityRepository userSecurityRepository,
            ICardRepository cardRepository, 
            IVignetteRepository vignetteRepository, 
            IValidator<UserSecurityCreateViewModel> createUserSecurityValidator, 
            IValidator<VignetteViewModel> updateVignetteValidator,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _sheetRepository = sheetRepository;
            _userSecurityRepository = userSecurityRepository; 
            _cardRepository = cardRepository;
            _vignetteRepository = vignetteRepository; 
            _updateVignetteValidator = updateVignetteValidator;
            _mapper = mapper;
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

        public async Task<GenericResponse> GetUserAccountsWithSheets(Guid userId)
        {
            LoginResponseViewModel responseModel = new LoginResponseViewModel();

            responseModel.Accounts = new List<AccountDto>();

            IEnumerable<Account> userAccounts = await _accountRepository.GetActiveAccountByUserId(userId);

            foreach (Account account in userAccounts)
            {
                AccountDto accountDto = new AccountDto
                {
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

        public async Task<GenericResponse> DeleteCardWithVignettes(Guid cardId)
        {
            LoginResponseViewModel responseModel = new LoginResponseViewModel();

            Card cardFound = await _cardRepository.GetCardById(cardId); 

            if(cardFound == null) {
                return new GenericResponse
                {
                    Resolution = false,
                    Message = $"La carta con el id '{ cardId }', no existe."
                };
            }

            IEnumerable<Vignette> vignettes = await _vignetteRepository.GetVignetteByCardId(cardId);

            //Se elininan todas las viñetas relacionadas a la carta. 
            foreach(Vignette vignette in vignettes)
                await _vignetteRepository.DeleteVignette(vignette.Id); 
            

            //se elimina la carta
            await _cardRepository.DeleteCard(cardId); 

            return new GenericResponse {
                Resolution = true,
                Data = responseModel,
                Message = "Se elimina la carta con todas sus viñetas"
            };
        }

        public async Task<GenericResponse> GetSheetCardsWithVignettes(Guid sheetId)
        {
            int sumTotalAmount = 0;
            try
            {
                // Obtener las cards asociadas al sheetId
                IEnumerable<Card> cards = await _cardRepository.GetCardBySheetId(sheetId);

                // Crear el modelo que contiene las cards con sus vignettes
                var model = new SheetCardsWithVignetteViewModel
                {
                    Cards = new List<CardWithVignettesDTO>()
                };

                foreach (Card card in cards)
                {
                    // Obtener las vignettes asociadas a la card actual
                    IEnumerable<Vignette> vignettes = await _vignetteRepository.GetVignetteByCardId(card.Id);

                    foreach (var vignette in vignettes) {
                        // sumTotalAmount = vignette != null ? sumTotalAmount + vignette.Amount : sumTotalAmount;
                        sumTotalAmount = sumTotalAmount + vignette.Amount;

                    }

                    // Mapear la card con sus vignettes
                    var cardWithVignettes = new CardWithVignettesDTO
                    {
                        Id = card.Id,
                        Title = card.Title,
                        Description = card.Description,
                        CreationDate = card.CreationDate,
                        Color = card.Color,
                        TotalCardAmount = sumTotalAmount,
                        Vignettes = vignettes.Select(v => new VignetteDTO
                        {
                            Id = v.Id,
                            Description = v.Description,
                            Amount = v.Amount,
                            Color = v.Color,
                            Order = v.Order
                        }).ToList(), 
                    };

                    sumTotalAmount = 0;

                    // Agregar la card al modelo
                    model.Cards.Add(cardWithVignettes);
                }

                // Retornar el modelo en una respuesta genérica
                return new GenericResponse
                {
                    Resolution = true,
                    Message = "Cards and Vignettes fetched successfully.",
                    Data = model
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    Resolution = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task<GenericResponse> UpdateVignetteAndRecalculateTotal(VignetteViewModel model)
        {
            GenericResponse response = new GenericResponse();
            int sumTotalAmount = 0;

            try
            {
                FluentValidation.Results.ValidationResult validationResult = _updateVignetteValidator.Validate(model);

                if (!validationResult.IsValid)
                {
                    return new GenericResponse
                    {
                        Resolution = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                        Message = "Se encontraron errores de validación."
                    };
                }

                Vignette existingVignette = await _vignetteRepository.GetVignetteById(model.Id);

                if (existingVignette == null)
                {
                    response.Resolution = false;
                    response.Data = $"Vineta con el id '{model.Id}' no encontrada.";
                    return response;
                }

                if (existingVignette.Amount != model.Amount) {
                    Card card = await _cardRepository.GetCardById(model.CardId);
                }

                IEnumerable<Vignette> cardVignettes = await _vignetteRepository.GetVignetteByCardId(model.CardId);

                sumTotalAmount = cardVignettes.Sum(v => v.Amount);
                sumTotalAmount = sumTotalAmount - existingVignette.Amount + model.Amount;

                _mapper.Map(model, existingVignette);
                await _vignetteRepository.UpdateVignette(existingVignette);

                response.Resolution = true;
                response.Data = new
                {
                    UpdatedVignette = existingVignette,
                    TotalAmount = sumTotalAmount
                };
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Data = ex.Message;
            }

            return response;
        }


        public async Task<GenericResponse> DeleteVignetteAndRecalculateTotal(Guid vignetteId)
        {
            GenericResponse response = new GenericResponse();
            int sumTotalAmount = 0;

            try
            {
                Vignette existingVignette = await _vignetteRepository.GetVignetteById(vignetteId);

                if (existingVignette == null)
                {
                    response.Resolution = false;
                    response.Data = $"Vineta con el id '{vignetteId}'no encontrado ";
                    return response;
                }

                bool resolution = await _vignetteRepository.DeleteVignette(vignetteId);
                response.Resolution = resolution;
                response.Message = (resolution) ? "Vineta eliminada" : "No se pudo eliminar el registro.";
                
                IEnumerable<Vignette> cardVignettes = await _vignetteRepository.GetVignetteByCardId(existingVignette.CardId);

                sumTotalAmount = cardVignettes.Sum(v => v.Amount);

                response.Resolution = true;
                response.Data = new {
                    TotalAmount = sumTotalAmount
                };
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Data = ex.Message;
            }

            return response;
        }

        public async Task<GenericResponse> UpdateVignetteColorTheme(Guid vignetteId, string colorTheme)
        {
            GenericResponse response = new GenericResponse();

            // FluentValidation.Results.ValidationResult validationResult = _updateVignetteValidator.Validate(vignetteId);

            // if (!validationResult.IsValid)
            // {
            //     return new GenericResponse
            //     {
            //         Resolution = false,
            //         Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
            //         Message = "Se encontraron errores de validación."
            //     };
            // }

            Vignette existingVignette = await _vignetteRepository.GetVignetteById(vignetteId);

            if (existingVignette == null)
            {
                response.Resolution = false;
                response.Data = $"Vineta con el id '{vignetteId}'no encontrado ";
                return response;
            }

            existingVignette.Color = colorTheme;
            await _vignetteRepository.UpdateVignette(existingVignette);

            response.Resolution = true;
            response.Data = new {
                UpdatedVignette = existingVignette,
            };

            return response;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}