﻿using AutoMapper;
using FluentValidation;
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

        public VignetteAppService(
            IVignetteRepository vignetteRepository,
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
            GenericResponse response = new GenericResponse();

            try
            {

                FluentValidation.Results.ValidationResult validationResult = _createVignetteValidator.Validate(model);

                if (!validationResult.IsValid)
                {
                    return new GenericResponse
                    {
                        Resolution = false,
                        Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray(),
                        Message = "Se encontraron errores de validación."
                    };
                }

                Card existingCard = await _cardRepository.GetCardById(model.CardId);

                if (existingCard == null){
                    response.Resolution = false;
                    response.Message = $"La carta con el id '{model.CardId}', no existe.";
                    return response;
                }

                Vignette vignette = _mapper.Map<Vignette>(model);
                vignette.Id = Guid.NewGuid();

                await _vignetteRepository.CreateVignette(vignette);
                response.Resolution = true;
                response.Data = vignette;
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Data = ex.Message;
            }

            return response;
        }

        public async Task<GenericResponse> UpdateVignette(VignetteViewModel model)
        {
            GenericResponse response = new GenericResponse();

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
                    response.Data = $"Vineta con el id '{ model.Id }'no encontrado ";
                    return response;
                }

                _mapper.Map(model, existingVignette);


                await _vignetteRepository.UpdateVignette(existingVignette);
                response.Resolution = true;
                response.Data = existingVignette;
            }
            catch (Exception ex)
            {
                response.Resolution = false;
                response.Data = ex.Message;
            }

            return response;
        }

        public async Task<GenericResponse> DeleteVignette(Guid id)
        {
            GenericResponse response = new GenericResponse();

            try
            {
                Vignette existingVignette = await _vignetteRepository.GetVignetteById(id);

                if (existingVignette == null)
                {
                    response.Resolution = false;
                    response.Data = $"Vineta con el id '{id}'no encontrado ";
                    return response;
                }

                bool resolution = await _vignetteRepository.DeleteVignette(id);
                response.Resolution = resolution;
                response.Message = (resolution) ? "Vineta eliminada" : "No se pudo eliminar el registro.";

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
