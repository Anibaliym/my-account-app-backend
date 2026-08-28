namespace MyAccountApp.Application.Responses
{
    public static class ErrorCodes
    {
        public const string CommonValidationFailed = "COMMON_VALIDATION_FAILED";
        public const string CommonUnexpectedError = "COMMON_UNEXPECTED_ERROR";
        public const string UserNotFound = "USER_NOT_FOUND";
        public const string AccountNotFound = "ACCOUNT_NOT_FOUND";
        public const string SheetNotFound = "SHEET_NOT_FOUND";
        public const string CardNotFound = "CARD_NOT_FOUND";
        public const string VignetteNotFound = "VIGNETTE_NOT_FOUND";
    }

    public static class ResponseMessages
    {
        public const string Success = "The request was completed successfully.";
        public const string ValidationFailed = "The request contains one or more validation errors.";
        public const string UnexpectedError = "An unexpected error occurred while processing the request.";
    }
}
