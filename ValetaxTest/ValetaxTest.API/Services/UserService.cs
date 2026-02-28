namespace ValetaxTest.API.Services;

public class UserService
{
    // Храним пользователей прямо в коде (для тестового задания)
    private readonly Dictionary<string, string> _users = new()
    {
        { "admin", "admin123" },
        { "test", "test123" },
        { "user", "password" }
    };

    public bool ValidateUser(string username, string password)
    {
        return _users.ContainsKey(username) && _users[username] == password;
    }
}