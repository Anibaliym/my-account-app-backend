namespace MyAccountApp.Application.Constants
{
    public static class ResponseMessages
    {
        public static class Common
        {
            public const string Success = "Request completed successfully.";
            public const string Created = "Resource created successfully.";
            public const string Updated = "Resource updated successfully.";
            public const string Deleted = "Resource deleted successfully.";
            public const string ValidationFailed = "The request contains validation errors.";
            public const string UnexpectedError = "An unexpected error occurred.";
            public const string ResourceNotFound = "The requested resource was not found.";
            public const string OperationFailed = "The operation is failed.";
        }

        public static class Authentication
        {
            public const string Success = "You have signed in successfully.";
            public const string Failed = "Authentication failed.";
            public const string InvalidCredentials = "Invalid email or password.";
            public const string ProviderMismatch = "This account must be accessed using Google Sign-In.";
            public static string GoogleSignInRequired(string email) => $"The account associated with '{ email }' was registered using Google Sign-In. Please sign in with Google.";
        }

        public static class Account
        {
            public const string Created = "The account was created successfully.";
            public const string Updated = "The account was updated successfully.";
            public const string Deleted = "The account was deleted successfully.";
            public static string DeletedAccount(string userEmail) => $"The user account associated with '{ userEmail }' has been deleted successfully."; 
            public static string NotFound(Guid accountId) => $"No account was found with id '{ accountId }'."; 
            public static string AccountOrderUpdated => "The vignette order has been updated successfully."; 
            public static string AccountCantBeDeleted => "The account cannot be deleted while it has associated sheets. Please remove the sheets first."; 
            public static string MaxAccountsReached(int maxAccounts) => $"The maximum number of accounts allowed per user is { maxAccounts }.";
            public const string LimitReached = "The maximum number of accounts allowed per user has been reached.";
            public const string HasAssociatedSheets = "The account cannot be deleted while it has associated sheets. Please remove the sheets first.";
        }

        public static class User
        {
            public static string NotFound(Guid userId) => $"The user with id '{ userId }' was not found."; 
            
        }

        public static class Card
        {
            public static string NotFound(Guid cardId) => $"No card was found with id '{cardId}'."; 
            public static string Deleted => $"The card has been deleted along with all its associated vignettes."; 
        }

        public static class Sheet
        {
            public static string NotFound(Guid sheetId) => $"No sheet was found with id '{sheetId}'."; 
            public static string Deleted(Guid sheetId) => $"The sheet with id '{sheetId}' and all its associated content have been deleted successfully."; 
            public const string BackupCreated = "The sheet backup was created successfully.";
        }

       public static class Vignette
        {
            public static string NotFound(Guid vignetteId) => $"No vignette was found with id '{ vignetteId }'."; 
            public static string Deleted(Guid vignetteId) => $"The vignette with id '{ vignetteId }' been deleted successfully."; 
            public static string DeletedFail(Guid vignetteId) => $"The vignette with id '{ vignetteId }' been deleted successfully."; 
            public const string Updated = "The vignette was updated successfully.";
            public const string ColorUpdated = "The vignette color theme was updated successfully.";
        }        
    }
}