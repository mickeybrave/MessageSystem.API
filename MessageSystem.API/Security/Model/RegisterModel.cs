namespace MessageSystem.API.Security.Model
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
