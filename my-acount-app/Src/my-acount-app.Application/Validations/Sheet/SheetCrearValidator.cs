using MyAccountApp.Application.ViewModels.Sheet;

namespace MyAccountApp.Application.Validations.Sheet
{
    public class SheetCrearValidator : BaseValidatorSheet<CreateSheetViewModel>
    {
        public SheetCrearValidator()
        {
            ValidateAccountId(sheet => sheet.AccountId);
            ValidateDescription(sheet => sheet.Description);
        }
    }
}
