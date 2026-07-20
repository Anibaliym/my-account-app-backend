namespace MyAccountApp.Application.Constants
{
    public static class ErrorCodes
    {
        public static class Common
        {
            public const string ValidationFailed = "COMMON_VALIDATION_FAILED";
            public const string ResourceNotFound = "COMMON_RESOURCE_NOT_FOUND";
            public const string ResourceAlreadyExists = "COMMON_RESOURCE_ALREADY_EXISTS";
            public const string UnexpectedError = "COMMON_UNEXPECTED_ERROR";
            public const string OperationFailed = "COMMON_OPERATION_FAILED";
            public const string OperationCancelled = "COMMON_OPERATION_CANCELLED";
        }

        public static class Authentication
        {
            public const string Failed = "AUTHENTICATION_FAILED";
            public const string InvalidCredentials = "AUTH_INVALID_CREDENTIALS";
            public const string GoogleSignInRequired = "AUTH_GOOGLE_SIGNIN_REQUIRED";
            public const string ProviderMismatch = "AUTH_PROVIDER_MISMATCH";
            public const string TokenExpired = "AUTH_TOKEN_EXPIRED";
            public const string TokenInvalid = "AUTH_TOKEN_INVALID";
            public const string AccessDenied = "AUTH_ACCESS_DENIED";
        }

        public static class Account
        {
            public const string LimitReached = "ACCOUNT_LIMIT_REACHED";
            public const string HasAssociatedSheets = "ACCOUNT_HAS_ASSOCIATED_SHEETS";            
        }

        public static class Card
        {
            public const string LimitReached = "CARD_LIMIT_REACHED";
        }

        public static class Sheet
        {
            public const string LimitReached = "SHEET_LIMIT_REACHED";
        }
    }
}