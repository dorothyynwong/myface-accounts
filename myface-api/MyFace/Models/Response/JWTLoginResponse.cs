using MyFace.Models.Database;

public class JWTLoginUserResponse
{
    private readonly User _user;
    private readonly string _token;

    public JWTLoginUserResponse(User user, string token)
    {
        _user = user;
        _token = token;
    }

    public int Id => _user.Id;
    public string FirstName => _user.FirstName;
    public string LastName => _user.LastName;
    public string DisplayName => $"{FirstName} {LastName}";
    public string Username => _user.Username;
    public string Token => _token;
}