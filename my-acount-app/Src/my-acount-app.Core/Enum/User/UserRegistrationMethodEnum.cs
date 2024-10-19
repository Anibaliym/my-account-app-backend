using Ardalis.SmartEnum;

namespace MyAccountApp.Core.Enum.User
{
    public sealed class UserRegistrationMethodEnum : SmartEnum<UserRegistrationMethodEnum>
    {
        public static readonly UserRegistrationMethodEnum MANUAL_AUTH = new UserRegistrationMethodEnum("MANUAL_AUTH", 1);
        public static readonly UserRegistrationMethodEnum GOOGLE_AUTH = new UserRegistrationMethodEnum("GOOGLE_AUTH", 2);

        private UserRegistrationMethodEnum(string name, int value) : base(name, value) { }

    }
}
