using AutoMapper;
using FluentValidation;
using MyAccountApp.Application.Constants;
using MyAccountApp.Application.Interfaces;
using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Card;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Interfaces;

namespace MyAccountApp.Application.Services
{
    public class CardAppService : ICardAppService
    {
        private readonly ICardRepository _cardRepository;
        private readonly ISheetRepository _sheetRepository;
        private readonly IValidator<CreateCardViewModel> _createCardValidator;
        private readonly IValidator<UpdateCardViewModel> _updateCardValidator;
        private readonly IMapper _mapper;
        private const int MaximumCardsPerSheet = 10;

        public CardAppService(IMapper mapper, ICardRepository cardRepository,ISheetRepository sheetRepository,IValidator<UpdateCardViewModel> updateCardValidator,IValidator<CreateCardViewModel> createCardValidator)
        {
            _mapper = mapper;
            _cardRepository = cardRepository;
            _sheetRepository = sheetRepository;
            _updateCardValidator = updateCardValidator;
            _createCardValidator = createCardValidator;
        }
        
        public async Task<CardViewModel> GetCardById(Guid id)
        {
            return _mapper.Map<CardViewModel>(await _cardRepository.GetCardById(id));
        }

        public async Task<IEnumerable<CardViewModel>> GetCardBySheetId(Guid idHoja)
        {
            return _mapper.Map<IEnumerable<CardViewModel>>(await _cardRepository.GetCardBySheetId(idHoja));
        }

        public async Task<GenericResponse> CreateCard(CreateCardViewModel model)
        {
            FluentValidation.Results.ValidationResult validationResult = await _createCardValidator.ValidateAsync(model);

            try
            {
                Guid sheetId = model.SheetId; 

                if (!validationResult.IsValid)
                {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ValidationFailed, 
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                        Message = ResponseMessages.Common.ValidationFailed
                    };
                }

                Sheet existingSheet = await _sheetRepository.GetSheetById(sheetId);

                if (existingSheet == null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound,
                        Message = ResponseMessages.Common.ResourceNotFound,
                        Errors = [ResponseMessages.Sheet.NotFound(sheetId)]
                    };
                }

                int totalCardsSheet = await _cardRepository.GetTotalCardsSheet(sheetId); 

                if (totalCardsSheet >= MaximumCardsPerSheet){
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Card.LimitReached,
                        Message = ResponseMessages.Common.OperationFailed,
                        Errors = [ResponseMessages.Card.LimitReached]
                    };                   
                }

                int order = await _cardRepository.GetNextOrderBySheetId(sheetId);
                
                Card card = _mapper.Map<Card>(model);
                card.Id = Guid.NewGuid();
                card.CreationDate = DateTime.UtcNow;
                card.Order = order;

                await _cardRepository.CreateCard(card);

                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Common.Created,
                    Data = _mapper.Map<CardViewModel>(card)
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

        public async Task<GenericResponse> UpdateCard(UpdateCardViewModel model)
        {
            FluentValidation.Results.ValidationResult validationResult = await _updateCardValidator.ValidateAsync(model);
            
            try
            {

                if (!validationResult.IsValid) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ValidationFailed,
                        Message = ResponseMessages.Common.ValidationFailed,
                        Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToArray()
                    };
                }

                Card existingCard = await _cardRepository.GetCardById(model.Id);
                
                if (existingCard == null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound,
                        Message = ResponseMessages.Common.ResourceNotFound,
                        Errors = [ResponseMessages.Card.NotFound(model.Id)]
                    };
                }

                Sheet existingSheet = await _sheetRepository.GetSheetById(model.SheetId);

                if (existingSheet == null) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound,
                        Message = ResponseMessages.Common.ResourceNotFound,
                        Errors = [ResponseMessages.Sheet.NotFound(model.SheetId)]
                    };
                }

                // Mapear solo las propiedades necesarias desde el modelo
                _mapper.Map(model, existingCard);

                // Asegúrate de que FechaCreacion está en UTC
                existingCard.CreationDate = existingCard.CreationDate.ToUniversalTime();

                await _cardRepository.UpdateCard(existingCard);


                return new GenericResponse {
                    Resolution = true,
                    Message = ResponseMessages.Common.Updated, 
                    Data = _mapper.Map<CardViewModel>(existingCard),
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

        public async Task<GenericResponse> UpdateCardOrderItems(List<UpdateCardViewModel> model)
        {
            try
            {
                foreach(UpdateCardViewModel card in model) 
                {
                    Guid cardId = card.Id; 

                    Card? obtainedCard = await _cardRepository.GetCardById(cardId);

                    if (obtainedCard == null) {
                        return new GenericResponse {
                            Resolution = false,
                            ErrorCode = ErrorCodes.Common.ResourceNotFound,
                            Message = ResponseMessages.Common.ResourceNotFound,
                            Errors = [ResponseMessages.Card.NotFound(cardId)]
                        };
                    }

                    obtainedCard.Order = card.Order; 
                    
                    await _cardRepository.UpdateCard(obtainedCard);
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

        public async Task<GenericResponse> DeleteCard(Guid id)
        {
            try
            {
                Card existingCard = await _cardRepository.GetCardById(id);
                
                if (existingCard == null)
                {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.ResourceNotFound, 
                        Message = ResponseMessages.Common.ResourceNotFound,
                        Errors = [ResponseMessages.Card.NotFound(id)], 
                    };
                }

                bool resolution = await _cardRepository.DeleteCard(id);

                if (!resolution) {
                    return new GenericResponse {
                        Resolution = false,
                        ErrorCode = ErrorCodes.Common.OperationFailed,
                        Message = ResponseMessages.Common.OperationFailed, 
                        Errors = [ ResponseMessages.Common.OperationFailed ]
                    };
                }

                return new GenericResponse {
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
