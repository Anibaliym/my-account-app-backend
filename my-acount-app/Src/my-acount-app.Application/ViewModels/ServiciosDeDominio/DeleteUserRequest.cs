namespace MyAccountApp.Application;

public class DeleteUserRequest
{
    public Guid UserId { get; set; }
    public string Password { get; set; } = string.Empty;
}