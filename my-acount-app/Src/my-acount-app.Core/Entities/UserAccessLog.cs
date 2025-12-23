namespace MyAccountApp.Core.Entities
{
    public class UserAccessLog
    {
        public Guid? Id { get; set; }

        /// <summary>
        /// Puede ser null cuando el usuario no existe o no se pudo resolver
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Fecha y hora exacta del evento (UTC)
        /// </summary>
        public DateTimeOffset OccurredAt { get; set; }

        /// <summary>
        /// LOGIN_SUCCESS, LOGIN_FAILED, LOGOUT, TOKEN_REFRESHED, SESSION_EXPIRED
        /// </summary>
        public string EventType { get; set; } = string.Empty;

        /// <summary>
        /// Indica si la operación fue exitosa
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// INVALID_PASSWORD, USER_NOT_FOUND, ACCOUNT_LOCKED, etc.
        /// Null cuando Success = true
        /// </summary>
        public string? FailureReason { get; set; }

        /// <summary>
        /// Dirección IP del cliente
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        /// User-Agent del navegador o app
        /// </summary>
        public string? UserAgent { get; set; }

        /// <summary>
        /// Google, Apple, Manual
        /// </summary>
        public string? AuthProvider { get; set; }
    }
}