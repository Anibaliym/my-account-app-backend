﻿using AutoMapper;
using FluentValidation;
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

        public SheetAppService(
            IMapper mapper,
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
            GenericResponse response = new GenericResponse();

            FluentValidation.Results.ValidationResult validationResult = _createSheetValidator.Validate(model);

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
                Account cuentaExistente = await _accountRepository.GetAccountById(model.AccountId);
                
                if (cuentaExistente == null)
                {
                    response.Resolution = false;
                    response.Message = $"La cuenta con el id '{model.AccountId}', no existe.";
                    return response;
                }

                int totalSheetsAccount = await _sheetRepository.GetTotalSheetsAccount(model.AccountId); 

                if (totalSheetsAccount >= 15){
                    response.Resolution = false;
                    response.Message = $"No se pueden crear mas de 15 hojas de calculo por cuenta.";
                    return response;
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
                response.Resolution = true;
                response.Data = sheet;
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<GenericResponse> UpdateSheet(UpdateSheetViewModel model)
        {
            GenericResponse response = new GenericResponse();

            FluentValidation.Results.ValidationResult validationResult = _updateSheetValidator.Validate(model);

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
                Account existingAccount = await _accountRepository.GetAccountById(model.AccountId);

                if (existingAccount == null)
                {
                    response.Resolution = false;
                    response.Message = $"La cuenta con el id '{model.AccountId}', no existe.";
                    return response;
                }


                Sheet existingSheet = await _sheetRepository.GetSheetById(model.Id);
                if (existingSheet == null)
                {
                    response.Resolution = false;
                    response.Data = "Hoja no encontrada";
                    return response;
                }

                _mapper.Map(model, existingSheet);

                existingSheet.CreationDate = existingSheet.CreationDate.ToUniversalTime();

                await _sheetRepository.UpdateSheet(existingSheet);
                response.Resolution = true;
                response.Data = existingSheet;
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Data = ex.Message;
            }

            return response;
        }

        public async Task<GenericResponse> UpdateSheetOrderItems(List<UpdateSheetViewModel> model)
        {
            try
            {
                foreach(UpdateSheetViewModel sheet in model) {
                    Sheet obtainedSheet = await _sheetRepository.GetSheetById(sheet.Id);

                    obtainedSheet.Order = sheet.Order; 
                    obtainedSheet.CreationDate = obtainedSheet.CreationDate.ToUniversalTime();
                    await _sheetRepository.UpdateSheet(obtainedSheet);
                }
                
                return new GenericResponse
                {
                    Resolution = true,
                    Message = "Se actualizó el orden de las hojas de calculo correctamente."
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

        public async Task<GenericResponse> UpdateCashBalance(Guid sheetId, int newCashBalance)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                // Obtén la hoja existente por su ID
                Sheet existingSheet = await _sheetRepository.GetSheetById(sheetId);

                if (existingSheet == null)
                {
                    response.Resolution = false;
                    response.Message = "Hoja no encontrada.";
                    return response;
                }

                // Actualiza solo el campo CashBalance
                existingSheet.CashBalance = newCashBalance;
                existingSheet.CreationDate = existingSheet.CreationDate.ToUniversalTime();

                // Usa el método UpdateSheet del repositorio para guardar los cambios
                await _sheetRepository.UpdateSheet(existingSheet);

                response.Resolution = true;
                response.Message = "CashBalance actualizado correctamente.";
                response.Data = existingSheet;
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<GenericResponse> UpdateCurrenteAccountBalance(Guid sheetId, int currentAccountBalance)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                Sheet existingSheet = await _sheetRepository.GetSheetById(sheetId);

                if (existingSheet == null)
                {
                    response.Resolution = false;
                    response.Message = "Hoja no encontrada.";
                    return response;
                }

                existingSheet.CurrentAccountBalance = currentAccountBalance;
                existingSheet.CreationDate = existingSheet.CreationDate.ToUniversalTime();

                await _sheetRepository.UpdateSheet(existingSheet);

                response.Resolution = true;
                response.Message = "CurrentAccountBalance actualizado correctamente.";
                response.Data = existingSheet;
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public async Task<GenericResponse> DeleteSheet(Guid id)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                Sheet hojaExistente = await _sheetRepository.GetSheetById(id);
                if (hojaExistente == null)
                {
                    response.Resolution = false;
                    response.Data = "Hoja no encontrada";
                    return response;
                }

                bool resolution = await _sheetRepository.DeleteSheet(id);
                response.Resolution = resolution;
                response.Message = (resolution) ? "Hoja eliminada" : "No se pudo eliminar el registro.";
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Data = ex.Message;
            }
            return response;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
