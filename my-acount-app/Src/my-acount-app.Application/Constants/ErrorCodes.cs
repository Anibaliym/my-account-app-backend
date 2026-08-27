namespace MyAccountApp.Application.Constants
{
    public static class ErrorCodes
    {
        public static class Common
        {
            public const string ValidationFailed = "COMMON_VALIDATION_FAILED";
            public const string UnexpectedError = "COMMON_UNEXPECTED_ERROR";
            public const string OperationFailed = "COMMON_OPERATION_FAILED";
            public const string OperationCancelled = "COMMON_OPERATION_CANCELLED";
        }

        public static class Authentication
        {
            public const string InvalidCredentials = "AUTH_INVALID_CREDENTIALS";
            public const string GoogleSignInRequired = "AUTH_GOOGLE_SIGNIN_REQUIRED";
        }

        public static class User
        {
            public const string NotFound = "USER_NOT_FOUND";
            public const string EmailAlreadyExists = "USER_EMAIL_ALREADY_EXISTS";
        }

        public static class Account
        {
            public const string NotFound = "ACCOUNT_NOT_FOUND";
            public const string AlreadyExists = "ACCOUNT_ALREADY_EXISTS";
            public const string LimitReached = "ACCOUNT_LIMIT_REACHED";
            public const string HasAssociatedSheets = "ACCOUNT_HAS_ASSOCIATED_SHEETS";
        }

        public static class Sheet
        {
            public const string NotFound = "SHEET_NOT_FOUND";
            public const string AlreadyExists = "SHEET_ALREADY_EXISTS";
            public const string LimitReached = "SHEET_LIMIT_REACHED";
            public const string HasAssociatedCards = "SHEET_HAS_ASSOCIATED_CARDS";
        }

        public static class Card
        {
            public const string NotFound = "CARD_NOT_FOUND";
            public const string AlreadyExists = "CARD_ALREADY_EXISTS";
            public const string LimitReached = "CARD_LIMIT_REACHED";
        }

        public static class Vignette
        {
            public const string NotFound = "VIGNETTE_NOT_FOUND";
            public const string LimitReached = "VIGNETTE_LIMIT_REACHED";
        }
    }
}