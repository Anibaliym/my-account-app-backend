﻿using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Sheet;

namespace MyAccountApp.Application.Interfaces
{
    public interface ISheetAppService : IDisposable
    {
        Task<SheetViewModel> GetSheetById(Guid id);
        Task<IEnumerable<SheetViewModel>> GetSheetByAccountId(Guid accountId);
        Task<GenericResponse> CreateSheet(CreateSheetViewModel model);
        Task<GenericResponse> UpdateSheet(UpdateSheetViewModel model);
        Task<GenericResponse> DeleteSheet(Guid id);
    }
}