using Ardalis.SmartEnum;

namespace MyAccountApp.Core.Enum
{
    public static partial class EnumValidator
    {
        public static bool ValidadorDeEnumeraciones<T>(string nombreEnumeracion) where T : SmartEnum<T>
        {
            bool result = SmartEnum<T>.TryFromName(nombreEnumeracion, out _);
            return result;
        }
    }
}
