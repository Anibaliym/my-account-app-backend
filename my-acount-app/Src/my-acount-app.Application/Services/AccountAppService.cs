using AutoMapper;
using FluentValidation;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Account;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Interfaces;

namespace MyAccountApp.Application.Services
{
    public class AccountAppService : IAccountAppService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISheetRepository _sheetRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateAccountViewModel> _createAccountValidator;
        private readonly IValidator<UpdateAccountViewModel> _updateAccountValidator;

        private readonly IMapper _mapper;

        public AccountAppService(
            IMapper mapper, 
            IAccountRepository accountRepository,
            IUserRepository userRepository,
            ISheetRepository sheetRepository,
            IValidator<CreateAccountViewModel> createAccountValidator,
            IValidator<UpdateAccountViewModel> updateAccountValidator
        )
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _sheetRepository = sheetRepository;
            _createAccountValidator = createAccountValidator;
            _updateAccountValidator = updateAccountValidator;
        }
        public async Task<AccountViewModel> GetActiveAccountById(Guid id)
        {
            return _mapper.Map<AccountViewModel>(await _accountRepository.GetActiveAccountById(id));
        }
        public async Task<IEnumerable<AccountViewModel>> GetActiveAccountByUserId(Guid userId)
        {
            return _mapper.Map<IEnumerable<AccountViewModel>>(await _accountRepository.GetActiveAccountByUserId(userId));
        }
        public async Task<GenericResponse> CreateAccount(CreateAccountViewModel model)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                FluentValidation.Results.ValidationResult validationResult = _createAccountValidator.Validate(model);

                if (!validationResult.IsValid)
                {
                    return new GenericResponse
                    {
                        Resolution = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                        Message = "Se encontraron errores de validación."
                    };
                }

                User user = await _userRepository.GetActiveUserById(model.UserId);

                if (user == null)
                {
                    response.Resolution = false;
                    response.Message = $"Usuario con el id {model.UserId}, no existe.";
                    return response;
                }

                int totalUserAccounts = await _accountRepository.GetTotalUserAccounts(model.UserId); 

                if (totalUserAccounts >= 15){
                    response.Resolution = false;
                    response.Message = $"No se pueden crear mas de 15 cuentas por usuario.";
                    return response;
                }

                int order = await _accountRepository.GetNextAccountOrderByUserId(model.UserId);

                Account account = _mapper.Map<Account>(model);
                account.Id = Guid.NewGuid();
                account.CreationDate = DateTime.UtcNow;
                account.IsActive = true;
                account.Order = order;

                await _accountRepository.CreateAccount(account);
                response.Resolution = true;
                response.Data = account;
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Message = ex.Message;
            }

            return response;
        }
        public async Task<GenericResponse> UpdateAccount(UpdateAccountViewModel model)
        {
            GenericResponse response = new GenericResponse();
            int order = 0; 

            FluentValidation.Results.ValidationResult validationResult = _updateAccountValidator.Validate(model);

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
                Account existingAccount = await _accountRepository.GetActiveAccountById(model.Id);

                if (existingAccount == null) {
                    response.Resolution = false;
                    response.Message = $"La cuenta con el id '{model.Id}', no existe.";
                    return response;
                }

                order = existingAccount.Order; 

                // Mapear solo las propiedades necesarias desde el modelo
                _mapper.Map(model, existingAccount);

                existingAccount.CreationDate = existingAccount.CreationDate.ToUniversalTime();
                existingAccount.Order = order;

                await _accountRepository.UpdateAccount(existingAccount);
                response.Resolution = true;
                response.Data = existingAccount;
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Message = ex.Message;
            }

            return response;
        }
        public async Task<GenericResponse> UpdateAccountOrderItems(List<UpdateAccountViewModel> model)
        {
            try
            {
                foreach(UpdateAccountViewModel account in model) {
                    Account obtainedAccount = await _accountRepository.GetActiveAccountById(account.Id);

                    obtainedAccount.Order = account.Order; 
                    
                    await _accountRepository.UpdateAccount(obtainedAccount);
                }
                
                return new GenericResponse
                {
                    Resolution = true,
                    Message = "Se actualizó el orden de las viñetas correctamente."
                };
            }
            catch (Exception error)
            {
                return new GenericResponse {
                    Resolution = false,
                    Message = error.Message
                };
            }
        }
        
        public async Task<GenericResponse> DeleteAccount(Guid id)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                Account existingAccount = await _accountRepository.GetActiveAccountById(id);
                IEnumerable<Sheet> sheetsAccount = await _sheetRepository.GetSheetByAccountId(id); 


                if(sheetsAccount.Count() > 0) { 
                    response.Resolution = false;
                    response.Message = $"No se puede eliminar la cuenta, por que tiene hojas de cálculo asociadas. Primero debe eliminar las hojas de cálculo.";
                    return response;
                }

                if (existingAccount == null)
                {
                    response.Resolution = false;
                    response.Message = $"la cuenta con el id '{id}', no existe.";
                    return response;
                }

                bool resolution = await _accountRepository.DeleteAccount(id);
                response.Resolution = resolution;
                response.Message = (resolution) ? "Cuenta eliminada" : "No se pudo eliminar el registro.";
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
