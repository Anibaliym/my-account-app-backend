using MyAccountApp.Application.ViewModels.Sheet;

namespace MyAccountApp.Application.Validations.Sheet
{
    public class SheetActualizarValidator : BaseValidatorSheet<UpdateSheetViewModel>
    {
        public SheetActualizarValidator()
        {
            ValidateId(sheet => sheet.Id);
            ValidateAccountId(sheet => sheet.AccountId);
            ValidateDescription(sheet => sheet.Description);
            ValidateOrder(Sheet => Sheet.Order); 
        }
    }
}
