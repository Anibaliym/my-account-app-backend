using Ardalis.SmartEnum;

namespace MyAccountApp.Core.Enum.UserAccessLog
{
    public sealed class UserAccessLogEventType : SmartEnum<UserAccessLogEventType>
    {
        public static readonly UserAccessLogEventType LOGIN_SUCCESS = new UserAccessLogEventType("LOGIN_SUCCESS", 1);
        public static readonly UserAccessLogEventType LOGIN_FAILED = new UserAccessLogEventType("LOGIN_FAILED", 2);
        public static readonly UserAccessLogEventType LOGOUT = new UserAccessLogEventType("LOGOUT", 3);
        public static readonly UserAccessLogEventType TOKEN_REFRESHED = new UserAccessLogEventType("TOKEN_REFRESHED", 4);
        public static readonly UserAccessLogEventType SESSION_EXPIRED = new UserAccessLogEventType("SESSION_EXPIRED", 5);
        
        private UserAccessLogEventType(string name, int value) : base(name, value) { }
    }
}