using Ardalis.SmartEnum;

namespace MyAccountApp.Core.Enum.Color
{
    public sealed class TipoColorEnum : SmartEnum<TipoColorEnum>
    {
        public static readonly TipoColorEnum DEFAULT = new TipoColorEnum("DEFAULT", 1);
        public static readonly TipoColorEnum GREEN = new TipoColorEnum("GREEN", 2);
        public static readonly TipoColorEnum YELLOW = new TipoColorEnum("YELLOW", 3);
        public static readonly TipoColorEnum ORANGE = new TipoColorEnum("ORANGE", 4);
        public static readonly TipoColorEnum RED = new TipoColorEnum("RED", 5);
        public static readonly TipoColorEnum BLUE = new TipoColorEnum("BLUE", 6);
        public static readonly TipoColorEnum GRAY = new TipoColorEnum("GRAY", 7);
        public static readonly TipoColorEnum PURPLE = new TipoColorEnum("PURPLE", 8);

        public static readonly TipoColorEnum SOFT_GREEN = new TipoColorEnum("SOFT_GREEN", 9);
        public static readonly TipoColorEnum SOFT_YELLOW = new TipoColorEnum("SOFT_YELLOW", 10);
        public static readonly TipoColorEnum SOFT_ORANGE = new TipoColorEnum("SOFT_ORANGE", 11);
        public static readonly TipoColorEnum SOFT_RED = new TipoColorEnum("SOFT_RED", 12);
        public static readonly TipoColorEnum SOFT_BLUE = new TipoColorEnum("SOFT_BLUE", 13);
        public static readonly TipoColorEnum SOFT_GRAY = new TipoColorEnum("SOFT_GRAY", 14);
        public static readonly TipoColorEnum SOFT_PURPLE = new TipoColorEnum("SOFT_PURPLE", 15);

        public static readonly TipoColorEnum STRONG_GREEN = new TipoColorEnum("STRONG_GREEN", 16);
        public static readonly TipoColorEnum STRONG_YELLOW = new TipoColorEnum("STRONG_YELLOW", 17);
        public static readonly TipoColorEnum STRONG_ORANGE = new TipoColorEnum("STRONG_ORANGE", 18);
        public static readonly TipoColorEnum STRONG_RED = new TipoColorEnum("STRONG_RED", 19);
        public static readonly TipoColorEnum STRONG_BLUE = new TipoColorEnum("STRONG_BLUE", 20);
        public static readonly TipoColorEnum STRONG_GRAY = new TipoColorEnum("STRONG_GRAY", 21);
        public static readonly TipoColorEnum STRONG_PURPLE = new TipoColorEnum("STRONG_PURPLE", 22);
        private TipoColorEnum(string name, int value) : base(name, value) { }
    }
}
