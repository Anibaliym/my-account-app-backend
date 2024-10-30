using MyAccountApp.Core.Entities;

namespace MyAccountApp.Application.Services 
{
    public class LoginResponseViewModel
    {
        public User? User { get; set; }
        public List<AccountDto>? Accounts { get; set; }
    }

    public class AccountDto
    {
        public Account? Account { get; set;}
        public List<SheetDto>? Sheets { get; set; }
    }

    public class SheetDto
    {
        public Sheet? Sheet { get; set; }
    }    
}

