using AutoMapper;
using FluentValidation;
using MyAccountApp.Application.Constants;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Sheet;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Interfaces;

namespace MyAccountApp.Application.Services
{
    public class SheetAppService : ISheetAppService
    {
        private readonly ISheetRepository _sheetRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IValidator<CreateSheetViewModel> _createSheetValidator;
        private readonly IValidator<UpdateSheetViewModel> _updateSheetValidator;
        private readonly IMapper _mapper;
        private const int MaximumSheetsPerAccount = 15;

        public SheetAppService(IMapper mapper, 
            ISheetRepository sheetRepository, 
            IAccountRepository accountRepository, 
            IValidator<CreateSheetViewModel> createSheetValidator, 
            IValidator<UpdateSheetViewModel> updateSheetValidator
        )
        {
            _mapper = mapper;
            _accountRepository = accountRepository;
            _sheetRepository = sheetRepository;
            _createSheetValidator = createSheetValidator;
            _updateSheetValidator = updateSheetValidator; 
        }

        public async Task<SheetViewModel> GetSheetById(Guid id)
        {
            return _mapper.Map<SheetViewModel>(await _sheetRepository.GetSheetById(id));
        }

        public async Task<SheetViewModel> GetSheetAccountByOrder(int order, Guid accountid)
        {
            return _mapper.Map<SheetViewModel>(await _sheetRepository.GetSheetAccountByOrder(order, accountid));
        }

        public async Task<IEnumerable<SheetViewModel>> GetSheetByAccountId(Guid accountId)
        {
            return _mapper.Map<IEnumerable<SheetViewModel>>(await _sheetRepository.GetSheetByAccountId(accountId));
        }

        public async Task<GenericResponse> CreateSheet(CreateSheetViewModel model)
        {
            FluentValidation.Results.ValidationResult validationResult = await _createSheetValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                return new GenericResponse {
                    Resolution = false,
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                    ErrorCode = ErrorCodes.Common.ValidationFailed,
                    Message = ResponseMessages.Common.ValidationFailed
                };
            }

            try
            {
                Account existingAccount = await _accountRepository.GetAccountById(model.AccountId);
                
                if (existingAccount == null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound, 
                        Message = ResponseMessages.Account.NotFound(model.AccountId),
                        Errors = [ResponseMessages.Account.NotFound(model.AccountId)],
                    }; 
                }

                int totalSheetsAccount = await _sheetRepository.GetTotalSheetsAccount(model.AccountId); 

                if (totalSheetsAccount >= MaximumSheetsPerAccount) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Sheet.LimitReached,
                        Message = ResponseMessages.Sheet.LimitReached,
                        Errors = [ResponseMessages.Sheet.LimitReached],
                    }; 
                }

                //Se obtiene el orden de la hoja a crear
                int order = await _sheetRepository.GetNextOrderByAccountId(model.AccountId);

                Sheet sheet = _mapper.Map<Sheet>(model);
                sheet.Id = Guid.NewGuid();
                sheet.CreationDate = DateTime.UtcNow;
                sheet.Order = order;
                sheet.CurrentAccountBalance = 0; 
                sheet.CashBalance = 0;

                await _sheetRepository.CreateSheet(sheet);

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Common.Created, 
                    Data = sheet,
                }; 
            }
            catch (Exception ex)
            {
                return new GenericResponse {
                    Resolution = false,
                    ErrorCode = ErrorCodes.Common.UnexpectedError, 
                    Message = ResponseMessages.Common.UnexpectedError,
                };
            }
        }

        public async Task<GenericResponse> UpdateSheet(UpdateSheetViewModel model)
        {
            FluentValidation.Results.ValidationResult validationResult = await _updateSheetValidator.ValidateAsync(model);

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
                Account existingAccount = await _accountRepository.GetAccountById(model.AccountId);

                if (existingAccount == null)
                {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound, 
                        Message = ResponseMessages.Account.NotFound(model.AccountId),
                    };  
                }

                Sheet existingSheet = await _sheetRepository.GetSheetById(model.Id);
                
                if (existingSheet == null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound, 
                        Message = ResponseMessages.Sheet.NotFound(model.Id),
                        Errors = [ResponseMessages.Sheet.NotFound(model.Id)],
                    }; 
                }

                _mapper.Map(model, existingSheet);

                existingSheet.CreationDate = existingSheet.CreationDate.ToUniversalTime();

                await _sheetRepository.UpdateSheet(existingSheet);

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Common.Updated, 
                    Data = existingSheet,
                }; 
            }
            catch (Exception ex)
            {
                return new GenericResponse {
                    Resolution = false,
                    ErrorCode = ErrorCodes.Common.UnexpectedError, 
                    Message = ResponseMessages.Common.UnexpectedError, 
                }; 
            }
        }

        public async Task<GenericResponse> UpdateSheetOrderItems(List<UpdateSheetViewModel> model)
        {
            try
            {
                foreach(UpdateSheetViewModel sheet in model) {
                    Sheet obtainedSheet = await _sheetRepository.GetSheetById(sheet.Id);

                    if (obtainedSheet == null) {
                        return new GenericResponse {
                            Resolution = false,
                            ErrorCode = ErrorCodes.Common.ResourceNotFound,
                            Message = ResponseMessages.Common.ResourceNotFound,
                            Errors = [ ResponseMessages.Sheet.NotFound(sheet.Id) ]
                        };
                    }

                    obtainedSheet.Order = sheet.Order; 
                    obtainedSheet.CreationDate = obtainedSheet.CreationDate.ToUniversalTime();
                    
                    await _sheetRepository.UpdateSheet(obtainedSheet);
                }
                
                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Common.Updated, 
                };
            }
            catch (Exception error)
            {
                return new GenericResponse {
                    Resolution = false,
                    ErrorCode = ErrorCodes.Common.UnexpectedError, 
                    Message = ResponseMessages.Common.UnexpectedError, 
                };
            }
        }

        public async Task<GenericResponse> UpdateCashBalance(Guid sheetId, int newCashBalance)
        {
            try
            {
                // Obtén la hoja existente por su ID
                Sheet existingSheet = await _sheetRepository.GetSheetById(sheetId);

                if (existingSheet == null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound, 
                        Message = ResponseMessages.Common.ResourceNotFound, 
                    };
                }

                // Actualiza solo el campo CashBalance
                existingSheet.CashBalance = newCashBalance;
                existingSheet.CreationDate = existingSheet.CreationDate.ToUniversalTime();

                // Usa el método UpdateSheet del repositorio para guardar los cambios
                await _sheetRepository.UpdateSheet(existingSheet);

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Common.Updated, 
                    Data = existingSheet, 
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse {
                    Resolution = false,
                    ErrorCode = ErrorCodes.Common.UnexpectedError, 
                    Message = ResponseMessages.Common.UnexpectedError, 
                };
            }
        }

        public async Task<GenericResponse> UpdateCurrenteAccountBalance(Guid sheetId, int currentAccountBalance)
        {
            try
            {
                Sheet existingSheet = await _sheetRepository.GetSheetById(sheetId);

                if (existingSheet == null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound, 
                        Message = ResponseMessages.Common.ResourceNotFound, 
                        Errors = [ResponseMessages.Common.ResourceNotFound], 
                    };
                }

                existingSheet.CurrentAccountBalance = currentAccountBalance;
                existingSheet.CreationDate = existingSheet.CreationDate.ToUniversalTime();

                await _sheetRepository.UpdateSheet(existingSheet);

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Common.Updated,
                    Data = existingSheet 
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse {
                    Resolution = false,
                    ErrorCode = ErrorCodes.Common.UnexpectedError, 
                    Message = ResponseMessages.Common.UnexpectedError, 
                };
            }
        }

        public async Task<GenericResponse> DeleteSheet(Guid id)
        {
            try
            {
                Sheet existingSheet = await _sheetRepository.GetSheetById(id);

                if (existingSheet == null){
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound, 
                        Message = ResponseMessages.Common.ResourceNotFound, 
                        Errors = [ResponseMessages.Sheet.NotFound(id)]
                    };
                }

                bool deleted = await _sheetRepository.DeleteSheet(id);

                if (!deleted) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.OperationFailed,
                        Message = ResponseMessages.Common.OperationFailed, 
                        Errors = [ResponseMessages.Common.OperationFailed], 
                    };
                }

                return new GenericResponse  {
                    Resolution = true,
                    Message = ResponseMessages.Common.Deleted
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse {
                    Resolution = false,
                    ErrorCode = ErrorCodes.Common.UnexpectedError, 
                    Message = ResponseMessages.Common.UnexpectedError, 
                };
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
