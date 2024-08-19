using System.ComponentModel.DataAnnotations;

public class LoginUserRequest
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
}