namespace Ordering.Application.Models;

public class EmailSettings
{
    public required string ApiKey { get; set; }
    public required string Name { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
