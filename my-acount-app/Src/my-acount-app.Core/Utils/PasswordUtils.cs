using System.Security.Cryptography;

namespace MyAccountApp.Core.Utils
{
    public static class PasswordUtils
    {
        // Genera el hash de la contraseña y el salt
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Genera un salt aleatorio con PBKDF2
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;  // Genera un salt único
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));  // Genera el hash
            }
        }

        // verifica la contraseña
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))  // Usamos el mismo salt para verificar
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);  // Verifica si el hash coincide
            }
        }
    }
}
