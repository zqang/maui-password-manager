namespace PasswordManager.Models;

public class Credential
{
    public Guid id { get; set; }
    public string UserId { get; set; }
    
    public string Password { get; set; }
    
    public string Domain { get; set; }

    public bool IsActive { get; set; } = true;
    
    public string? Secret { get; set; }
    
    public string? PictureUri { get; set; }
}