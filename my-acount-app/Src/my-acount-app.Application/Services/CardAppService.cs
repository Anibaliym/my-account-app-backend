using AutoMapper;
using FluentValidation;
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

        public CardAppService(
            IMapper mapper, 
            ICardRepository cardRepository,
            ISheetRepository sheetRepository,
            IValidator<UpdateCardViewModel> updateCardValidator,
            IValidator<CreateCardViewModel> createCardValidator
        )
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
            GenericResponse response = new GenericResponse();

            try
            {
                FluentValidation.Results.ValidationResult validationResult = _createCardValidator.Validate(model);

                if (!validationResult.IsValid)
                {
                    return new GenericResponse
                    {
                        Resolution = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                        Message = "Se encontraron errores de validación."
                    };
                }

                Sheet existingSheet = await _sheetRepository.GetSheetById(model.SheetId);

                if (existingSheet == null)
                {
                    response.Resolution = false;
                    response.Data = $"No existe una 'Hoja' con el id '{model.SheetId}'";
                    return response;
                }

                int order = await _cardRepository.GetNextOrderBySheetId(model.SheetId);
                
                Card card = _mapper.Map<Card>(model);
                card.Id = Guid.NewGuid();
                card.CreationDate = DateTime.UtcNow;
                card.Order = order;

                await _cardRepository.CreateCard(card);
                response.Resolution = true;
                response.Data = card;
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Data = ex.Message;
            }

            return response;
        }

        public async Task<GenericResponse> UpdateCard(UpdateCardViewModel model)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                FluentValidation.Results.ValidationResult validationResult = _updateCardValidator.Validate(model);

                if (!validationResult.IsValid)
                {
                    return new GenericResponse {
                        Resolution = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                        Message = "Se encontraron errores de validación."
                    };
                }

                Card existingCard = await _cardRepository.GetCardById(model.Id);
                
                if (existingCard == null)
                {
                    response.Resolution = false;
                    response.Data = "Carta no encontrada";
                    return response;
                }

                Sheet existingSheet = await _sheetRepository.GetSheetById(model.SheetId);

                if (existingSheet == null)
                {
                    response.Resolution = false;
                    response.Data = $"No existe una 'Hoja' con el id '{ model.SheetId }'";
                    return response;
                }

                // Mapear solo las propiedades necesarias desde el modelo
                _mapper.Map(model, existingCard);

                // Asegúrate de que FechaCreacion está en UTC
                existingCard.CreationDate = existingCard.CreationDate.ToUniversalTime();

                await _cardRepository.UpdateCard(existingCard);
                response.Resolution = true;
                response.Data = existingCard;
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Data = ex.Message;
            }

            return response;
        }

        public async Task<GenericResponse> UpdateCardOrderItems(List<UpdateCardViewModel> model)
        {
            try
            {
                foreach(UpdateCardViewModel card in model) {
                    Card obtainedCard = await _cardRepository.GetCardById(card.Id);

                    obtainedCard.Order = card.Order; 
                    obtainedCard.CreationDate = obtainedCard.CreationDate.ToUniversalTime();
                    
                    await _cardRepository.UpdateCard(obtainedCard);
                }
                
                return new GenericResponse
                {
                    Resolution = true,
                    Message = "Se actualizó el orden de las cartas correctamente."
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

        public async Task<GenericResponse> DeleteCard(Guid id)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                Card existingCard = await _cardRepository.GetCardById(id);
                
                if (existingCard == null)
                {
                    response.Resolution = false;
                    response.Data = "Carta no encontrada";
                    return response;
                }

                bool resolution = await _cardRepository.DeleteCard(id);
                response.Resolution = resolution;
                response.Message = (resolution) ? "Carta eliminada" : "No se pudo eliminar el registro.";
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
