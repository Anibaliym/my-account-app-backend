namespace MyAccountApp.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public string UserType { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RegistrationMethod { get; set; } = string.Empty; 
    }
}
