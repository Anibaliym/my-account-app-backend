﻿using MyAccountApp.Application.ViewModels.Account;

namespace MyAccountApp.Application.Validations.Account
{
    public class AccountUpdateValidator : BaseValidatorAccount<UpdateAccountViewModel>
    {
        public AccountUpdateValidator()
        {
            ValidateId(account => account.Id);
            ValidateUserId(account => account.UserId);
            ValidateDescription(account => account.Description);
        }
    }
}