using Ardalis.SmartEnum;

namespace MyAccountApp.Core.Enum.UserAccessLog
{
    public sealed class  UserAccessLogFailureReason : SmartEnum<UserAccessLogFailureReason>
    {
        public static readonly UserAccessLogFailureReason INVALID_PASSWORD = new UserAccessLogFailureReason("INVALID_PASSWORD", 1);
        public static readonly UserAccessLogFailureReason USER_NOT_FOUND = new UserAccessLogFailureReason("USER_NOT_FOUND", 2);
        public static readonly UserAccessLogFailureReason ACCOUNT_ALOWED = new UserAccessLogFailureReason("ACCOUNT_ALOWED", 3);
        public static readonly UserAccessLogFailureReason ACCOUNT_LOCKED = new UserAccessLogFailureReason("ACCOUNT_LOCKED", 4);
        public static readonly UserAccessLogFailureReason TOO_MANY_ATTEMPTS = new UserAccessLogFailureReason("TOO_MANY_ATTEMPTS", 5);
        public static readonly UserAccessLogFailureReason SESSION_EXPIRED = new UserAccessLogFailureReason("SESSION_EXPIRED", 6);
        public static readonly UserAccessLogFailureReason TOKEN_EXPIRED = new UserAccessLogFailureReason("TOKEN_EXPIRED", 7);
        public static readonly UserAccessLogFailureReason TOKEN_INVALID = new UserAccessLogFailureReason("TOKEN_INVALID", 8);
        
        private UserAccessLogFailureReason(string name, int value) : base(name, value) { }
    }   
}