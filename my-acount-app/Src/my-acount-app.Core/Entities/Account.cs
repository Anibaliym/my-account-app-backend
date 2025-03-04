namespace MyAccountApp.Core.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
    }
}
