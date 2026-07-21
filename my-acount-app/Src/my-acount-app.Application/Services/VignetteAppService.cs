using AutoMapper;
using FluentValidation;
using MyAccountApp.Application.Constants;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Vignette;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Interfaces;

namespace MyAccountApp.Application.Services
{
    public class VignetteAppService : IVignetteAppService
    {
        private readonly IVignetteRepository _vignetteRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IValidator<VignetteCreateViewModel> _createVignetteValidator;
        private readonly IValidator<VignetteViewModel> _updateVignetteValidator;
        private readonly IMapper _mapper;

        public VignetteAppService(IVignetteRepository vignetteRepository, 
            ICardRepository cardRepository, 
            IMapper mapper, 
            IValidator<VignetteCreateViewModel> createVignetteValidator, 
            IValidator<VignetteViewModel> updateVignetteValidator
        )
        {
            _cardRepository = cardRepository;
            _vignetteRepository = vignetteRepository;
            _createVignetteValidator = createVignetteValidator;
            _updateVignetteValidator = updateVignetteValidator;
            _mapper = mapper;
        }
        public async Task<VignetteViewModel> GetVignetteById(Guid id)
        {
            return _mapper.Map<VignetteViewModel>(await _vignetteRepository.GetVignetteById(id));
        }
        public async Task<IEnumerable<VignetteViewModel>> GetVignetteByCardId(Guid cardId)
        {
            return _mapper.Map<IEnumerable<VignetteViewModel>>(await _vignetteRepository.GetVignetteByCardId(cardId));
        }
        public async Task<GenericResponse> CreateVignette(VignetteCreateViewModel model)
        {
            try
            {
                FluentValidation.Results.ValidationResult validationResult = await _createVignetteValidator.ValidateAsync(model);

                if (!validationResult.IsValid) {
                    return new GenericResponse {
                        Resolution = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                        Message = ResponseMessages.Common.ValidationFailed, 
                    };
                }

                //Se valida que la carta relacionada exista. 
                Card existingCard = await _cardRepository.GetCardById(model.CardId);

                if (existingCard == null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound, 
                        Message = ResponseMessages.Card.NotFound(model.CardId),
                        Errors = [ResponseMessages.Card.NotFound(model.CardId)],
                    };
                }

                int totalVignettesCard = await _vignetteRepository.GetTotalVignettesCard(model.CardId); 

                if (totalVignettesCard >= 20) {

                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ValidationFailed, 
                        Message = ResponseMessages.Vignette.LimitReached,
                        Errors = [ ResponseMessages.Vignette.LimitReached ]
                    }; 
                }

                int order = await _vignetteRepository.GetNextOrderByCardId(model.CardId);

                Vignette vignette = _mapper.Map<Vignette>(model);
                vignette.Id = Guid.NewGuid();
                vignette.Order = order; 

                await _vignetteRepository.CreateVignette(vignette);

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Common.Created, 
                    Data = vignette,
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
        public async Task<GenericResponse> UpdateVignette(VignetteViewModel model)
        {
            try
            {
                FluentValidation.Results.ValidationResult validationResult = await _updateVignetteValidator.ValidateAsync(model);

                if (!validationResult.IsValid) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ValidationFailed, 
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                        Message = ResponseMessages.Common.ValidationFailed
                    };
                }

                Vignette existingVignette = await _vignetteRepository.GetVignetteById(model.Id);

                if (existingVignette == null)
                {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound, 
                        Message = ResponseMessages.Vignette.NotFound(model.Id), 
                        Errors = [ ResponseMessages.Vignette.NotFound(model.Id) ], 
                    }; 
                }

                _mapper.Map(model, existingVignette);

                await _vignetteRepository.UpdateVignette(existingVignette);

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Common.Updated, 
                    Data = existingVignette,
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
        public async Task<GenericResponse> UpdateVignetteOrderItems(List<UpdateVignetteViewModel> model)
        {
            try
            {
                foreach(UpdateVignetteViewModel vignette in model) {
                    Vignette obtainedVignette = await _vignetteRepository.GetVignetteById(vignette.Id);

                    obtainedVignette.Order = vignette.Order; 
                    
                    await _vignetteRepository.UpdateVignette(obtainedVignette);
                }
                
                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Common.Updated
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
        public async Task<GenericResponse> DeleteVignette(Guid id)
        {
            try
            {
                Vignette existingVignette = await _vignetteRepository.GetVignetteById(id);

                if (existingVignette == null)
                {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound, 
                        Message = ResponseMessages.Vignette.NotFound(id), 
                        Errors = [ ResponseMessages.Vignette.NotFound(id) ], 
                    }; 
                }

                bool resolution = await _vignetteRepository.DeleteVignette(id);

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
