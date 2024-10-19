using AutoMapper;
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

        public async Task<IEnumerable<SheetViewModel>> GetSheetByAccountId(Guid idCuenta)
        {
            return _mapper.Map<IEnumerable<SheetViewModel>>(await _sheetRepository.GetSheetByAccountId(idCuenta));
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
                Account cuentaExistente = await _accountRepository.GetActiveAccountById(model.AccountId);

                if (cuentaExistente == null)
                {
                    response.Resolution = false;
                    response.Message = $"La cuenta con el id '{model.AccountId}', no existe.";
                    return response;
                }

                Sheet sheet = _mapper.Map<Sheet>(model);
                sheet.Id = Guid.NewGuid();
                sheet.CreationDate = DateTime.UtcNow;


                await _sheetRepository.CreateSheet(sheet);
                response.Resolution = true;
                response.Data = sheet;
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Data = ex.Message;
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
                Account existingAccount = await _accountRepository.GetActiveAccountById(model.AccountId);

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
