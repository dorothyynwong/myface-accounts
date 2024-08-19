using MyFace.Models.Database;

public class LoginUserResponse
{
    private readonly User _user;

    public LoginUserResponse(User user)
    {
        _user = user;
    }

    public int Id => _user.Id;
    public string FirstName => _user.FirstName;
    public string LastName => _user.LastName;
    public string DisplayName => $"{FirstName} {LastName}";
    public string Username => _user.Username;
}