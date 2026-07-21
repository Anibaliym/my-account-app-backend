// 

namespace MyAccountApp.Application.Constants
{
    public static class ResponseMessages
    {
        public static class Common
        {
            public const string Success = "The request was completed successfully.";
            public const string Created = "The resource was created successfully.";
            public const string ValidationFailed = "The request contains one or more validation errors.";
            public const string UnexpectedError = "An unexpected error occurred while processing the request.";
            public const string UnexpectedException = "An unexpected exception occurred while processing the request.";
            public const string ResourceNotFound = "The requested resource was not found.";
            public const string OperationFailed = "The operation could not be completed.";
        }

        public static class Authentication
        {
            public const string Success = "You have signed in successfully.";
            public const string Failed = "Authentication could not be completed.";
            public const string InvalidCredentials = "The email address or password is incorrect.";
            public static string GoogleSignInRequired(string email) => $"The account associated with '{email}' was registered using Google Sign-In. Please sign in with Google.";
        }

        public static class Account
        {
            public const string Created = "The account was created successfully.";
            public const string Updated = "The account was updated successfully.";
            public const string Deleted = "The account was deleted successfully.";
            public static string NotFound(Guid accountId) => $"No account was found with the ID '{accountId}'.";
            public const string AccountOrderUpdated = "The account order was updated successfully.";
            public const string LimitReached = "The maximum number of accounts allowed per user has been reached.";
            public const string HasAssociatedSheets = "The account cannot be deleted because it has associated sheets. Please delete the associated sheets first.";
        }

        public static class User
        {
            public const string Created = "The user was created successfully.";
            public const string Updated = "The user was updated successfully.";
            public static string NotFound(Guid userId) => $"No user was found with the ID '{userId}'.";
            public static string Deleted(string email) => $"The user account associated with the email address '{email}' was deleted successfully.";
            public static string EmailAlreadyExists(string email) => $"The email address '{email}' is already associated with an existing user account.";
        }

        public static class Card
        {
            public const string Created = "The card was created successfully.";
            public const string Updated = "The card was updated successfully.";
            public const string OrderUpdated = "The card order was updated successfully.";
            public static string NotFound(Guid cardId) => $"No card was found with the ID '{cardId}'.";
            public const string Deleted = "The card and all its associated vignettes were deleted successfully.";
            public const string LimitReached = "The maximum number of 10 cards allowed per sheet has been reached.";
        }

        public static class Sheet
        {
            public const string Created = "The sheet was created successfully.";
            public const string Updated = "The sheet was updated successfully.";
            public const string OrderUpdated = "The sheet order was updated successfully.";
            public const string BackupCreated = "The sheet backup was created successfully.";
            public const string CashBalanceUpdated = "The cash balance was updated successfully.";
            public const string CurrentAccountBalanceUpdated = "The current account balance was updated successfully.";
            public static string NotFound(Guid sheetId) => $"No sheet was found with the ID '{sheetId}'.";
            public static string Deleted(Guid sheetId) => $"The sheet with the ID '{sheetId}' and all its associated content were deleted successfully.";
            public const string LimitReached = "The maximum number of 15 sheets allowed per account has been reached.";
        }

        public static class Vignette
        {
            public const string Created = "The vignette was created successfully.";
            public const string Updated = "The vignette was updated successfully.";
            public const string OrderUpdated = "The vignette order was updated successfully.";
            public const string ColorUpdated = "The vignette color theme was updated successfully.";
            public static string NotFound(Guid vignetteId) => $"No vignette was found with the ID '{vignetteId}'.";
            public static string Deleted(Guid vignetteId) => $"The vignette with the ID '{vignetteId}' was deleted successfully.";
            public static string DeleteFailed(Guid vignetteId) => $"The vignette with the ID '{vignetteId}' could not be deleted.";
            public const string LimitReached = "The maximum number of 20 vignettes allowed per card has been reached.";
        }
    }
}