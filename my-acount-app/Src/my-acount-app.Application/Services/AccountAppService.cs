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
            FluentValidation.Results.ValidationResult validationResult = await _createAccountValidator.ValidateAsync(model);

            try
            {
                if (!validationResult.IsValid) {
                    return new GenericResponse {
                        Resolution = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                        ErrorCode = ErrorCodes.Common.ValidationFailed,
                        Message = ResponseMessages.Common.ValidationFailed
                    };
                }

                User? user = await _userRepository.GetUserById(model.UserId);

                if (user == null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.User.NotFound,
                        Message = ResponseMessages.User.NotFound(model.UserId),
                        Errors = [ ResponseMessages.User.NotFound(model.UserId) ],
                    };
                }

                int totalUserAccounts = await _accountRepository.GetTotalUserAccounts(model.UserId); 

                if (totalUserAccounts >= 15) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Account.LimitReached,
                        Message = ResponseMessages.Account.LimitReached, 
                        Errors = [ ResponseMessages.Account.LimitReached ]
                    };
                }

                int order = await _accountRepository.GetNextAccountOrderByUserId(model.UserId);

                Account account = _mapper.Map<Account>(model);
                account.Id = Guid.NewGuid();
                account.CreationDate = DateTime.UtcNow;
                account.Order = order;

                await _accountRepository.CreateAccount(account);

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Account.Created, 
                    Data = account,
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
        public async Task<GenericResponse> UpdateAccount(UpdateAccountViewModel model)
        {
            int order;
            FluentValidation.Results.ValidationResult validationResult = await _updateAccountValidator.ValidateAsync(model);

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
                Account? existingAccount = await _accountRepository.GetAccountById(model.Id);

                if (existingAccount == null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Account.NotFound,
                        Message = ResponseMessages.Account.NotFound(model.Id),
                        Errors = [ ResponseMessages.Account.NotFound(model.Id) ]
                    }; 
                }

                order = existingAccount.Order; 

                // Mapear solo las propiedades necesarias desde el modelo
                _mapper.Map(model, existingAccount);

                existingAccount.CreationDate = existingAccount.CreationDate.ToUniversalTime();
                existingAccount.Order = order;

                await _accountRepository.UpdateAccount(existingAccount);

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Account.Updated,
                    Data = existingAccount,
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse {
                    Resolution = false,
                    ErrorCode = ErrorCodes.Common.UnexpectedError,
                    Message = ResponseMessages.Common.UnexpectedError
                };
            }
        }
        
        public async Task<GenericResponse> UpdateAccountOrderItems(List<UpdateAccountViewModel> model)
        {
            try
            {
                List<Account> accountsToUpdate = new List<Account>();

                foreach (UpdateAccountViewModel accountModel in model)
                {
                    Account? obtainedAccount = await _accountRepository.GetAccountById(accountModel.Id);

                    if (obtainedAccount == null) {
                        return new GenericResponse {
                            Resolution = false,
                            ErrorCode = ErrorCodes.Account.NotFound,
                            Message = ResponseMessages.Account.NotFound(accountModel.Id),
                            Errors = [ ResponseMessages.Account.NotFound(accountModel.Id) ]
                        };
                    }

                    obtainedAccount.Order = accountModel.Order;
                    accountsToUpdate.Add(obtainedAccount);
                }

                foreach (Account account in accountsToUpdate) {
                    await _accountRepository.UpdateAccount(account);
                }

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Account.AccountOrderUpdated, 
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
                Account? existingAccount = await _accountRepository.GetAccountById(accountId);

                if (existingAccount == null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Account.NotFound,
                        Message = ResponseMessages.Account.NotFound(accountId),
                        Errors = [ResponseMessages.Account.NotFound(accountId)]
                    };
                }

                IEnumerable<Sheet> sheetsAccount = await _sheetRepository.GetSheetByAccountId(accountId);

                if (sheetsAccount.Any()) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Account.HasAssociatedSheets,
                        Message = ResponseMessages.Account.HasAssociatedSheets,
                        Errors = [ ResponseMessages.Account.HasAssociatedSheets ]
                    };
                }

                bool deleted = await _accountRepository.DeleteAccount(accountId);

                if (!deleted) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.OperationFailed,
                        Message = ResponseMessages.Common.OperationFailed, 
                        Errors = [ ResponseMessages.Common.OperationFailed ]
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
