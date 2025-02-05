﻿using MyAccountApp.Application.ViewModels.User;

namespace MyAccountApp.Application.Validations.User
{
    public class UserUpdateValidator : BaseValidatorUser<UserUpdateViewModel>
    {
        public UserUpdateValidator()
        {
            ValidateId(user => user.Id);
            ValidateFirstName(user => user.FirstName);
            ValidateLastName(user => user.LastName);
            ValidateEmail(user => user.Email);
            ValidateUserType(user => user.UserType);
        }
    }
}
