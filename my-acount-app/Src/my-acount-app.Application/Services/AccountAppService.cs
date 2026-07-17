using AutoMapper;
using FluentValidation;
using MyAccountApp.Application.Constants;
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
        public async Task<AccountViewModel> GetAccountById(Guid accountId)
        {
            return _mapper.Map<AccountViewModel>(await _accountRepository.GetAccountById(accountId));
        }
        public async Task<IEnumerable<AccountViewModel>> GetAccountByUserId(Guid userId)
        {
            return _mapper.Map<IEnumerable<AccountViewModel>>(await _accountRepository.GetAccountByUserId(userId));
        }
        public async Task<GenericResponse> CreateAccount(CreateAccountViewModel model)
        {
            GenericResponse response = new GenericResponse();
            Guid userId = model.UserId; 

            try
            {
                FluentValidation.Results.ValidationResult validationResult = _createAccountValidator.Validate(model);

                if (!validationResult.IsValid)
                {
                    return new GenericResponse
                    {
                        Resolution = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                        ErrorCode = ErrorCodes.Common.ValidationFailed,
                        Message = ResponseMessages.Common.ValidationFailed
                    };
                }

                User user = await _userRepository.GetUserById(userId);

                if (user == null)
                {
                    response.Resolution = false;
                    response.ErrorCode = ErrorCodes.Common.ResourceNotFound; 
                    response.Message = ResponseMessages.User.NotFound(userId); 

                    return response;
                }

                int totalUserAccounts = await _accountRepository.GetTotalUserAccounts(userId); 

                if (totalUserAccounts >= 15) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Account.LimitReached,
                        Message = ResponseMessages.Account.LimitReached
                    };
                }

                int order = await _accountRepository.GetNextAccountOrderByUserId(userId);

                Account account = _mapper.Map<Account>(model);
                account.Id = Guid.NewGuid();
                account.CreationDate = DateTime.UtcNow;
                account.Order = order;

                //ayanez
                await _accountRepository.CreateAccount(account);

                response.Resolution = true;
                response.Message = ResponseMessages.Account.Created; 
                response.Data = account;
            }
            catch (Exception)
            {
                return new GenericResponse {
                    Resolution = false,
                    ErrorCode = ErrorCodes.Common.UnexpectedError,
                    Message = ResponseMessages.Common.UnexpectedError
                };
            }

            return response;
        }
        public async Task<GenericResponse> UpdateAccount(UpdateAccountViewModel model)
        {
            GenericResponse response = new GenericResponse();
            Guid accountId = model.Id; 
            int order = 0; 

            FluentValidation.Results.ValidationResult validationResult = _updateAccountValidator.Validate(model);

            if (!validationResult.IsValid)
            {
                return new GenericResponse
                {
                    Resolution = false,
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                    ErrorCode = ErrorCodes.Common.ValidationFailed, 
                    Message = ResponseMessages.Common.ValidationFailed
                };
            }

            try
            {
                Account existingAccount = await _accountRepository.GetAccountById(accountId);

                if (existingAccount == null) {
                    response.Resolution = false;
                    response.ErrorCode = ErrorCodes.Common.ResourceNotFound; 
                    response.Message = ResponseMessages.Account.NotFound(accountId); 
                    return response;
                }

                order = existingAccount.Order; 

                // Mapear solo las propiedades necesarias desde el modelo
                _mapper.Map(model, existingAccount);

                existingAccount.CreationDate = existingAccount.CreationDate.ToUniversalTime();
                existingAccount.Order = order;

                await _accountRepository.UpdateAccount(existingAccount);

                response.Resolution = true;
                response.Message = ResponseMessages.Account.Updated; 
                response.Data = existingAccount;
            }
            catch (Exception)
            {
                return new GenericResponse {
                    Resolution = false,
                    ErrorCode = ErrorCodes.Common.UnexpectedError,
                    Message = ResponseMessages.Common.UnexpectedError
                };
            }

            return response;
        }
        
        public async Task<GenericResponse> UpdateAccountOrderItems(List<UpdateAccountViewModel> model)
        {
            try
            {
                List<Account> accountsToUpdate = new List<Account>();

                foreach (UpdateAccountViewModel accountModel in model)
                {
                    Account obtainedAccount = await _accountRepository.GetAccountById(accountModel.Id);

                    if (obtainedAccount == null) {
                        return new GenericResponse {
                            Resolution = false,
                            ErrorCode = ErrorCodes.Common.ResourceNotFound,
                            Message = ResponseMessages.Common.ResourceNotFound,
                            Errors = [ ResponseMessages.Account.NotFound(accountModel.Id) ]
                        };
                    }

                    obtainedAccount.Order = accountModel.Order;
                    accountsToUpdate.Add(obtainedAccount);
                }

                foreach (Account account in accountsToUpdate)
                    await _accountRepository.UpdateAccount(account);

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Account.AccountOrderUpdated
                };
            }
            catch (Exception)
            {
                return new GenericResponse {
                    Resolution = false,
                    ErrorCode = ErrorCodes.Common.UnexpectedError,
                    Message = ResponseMessages.Common.UnexpectedError
                };
            }
        }
        
        public async Task<GenericResponse> DeleteAccount(Guid accountId)
        {
            try
            {
                Account existingAccount = await _accountRepository.GetAccountById(accountId);

                if (existingAccount == null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound,
                        Message = ResponseMessages.Common.ResourceNotFound,
                        Errors = [ResponseMessages.Account.NotFound(accountId)]
                    };
                }

                IEnumerable<Sheet> sheetsAccount = await _sheetRepository.GetSheetByAccountId(accountId);

                if (sheetsAccount.Any()) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Account.HasAssociatedSheets,
                        Message = ResponseMessages.Account.HasAssociatedSheets
                    };
                }

                bool deleted = await _accountRepository.DeleteAccount(accountId);

                if (!deleted) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.OperationFailed,
                        Message = ResponseMessages.Common.OperationFailed
                    };
                }

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Account.Deleted
                };
            }
            catch (Exception)
            {
                return new GenericResponse
                {
                    Resolution = false,
                    ErrorCode = ErrorCodes.Common.UnexpectedError,
                    Message = ResponseMessages.Common.UnexpectedError
                };
            }
        }        

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
