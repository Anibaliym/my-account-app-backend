using Ardalis.SmartEnum;

namespace MyAccountApp.Core.Enum.User
{
    public sealed class UserTypeEnum : SmartEnum<UserTypeEnum>
    {
        public static readonly UserTypeEnum STANDAR_USER = new UserTypeEnum("STANDAR_USER", 1);
        public static readonly UserTypeEnum ADMIN = new UserTypeEnum("ADMIN", 2);

        private UserTypeEnum(string name, int value) : base(name, value) { }
    }
}

