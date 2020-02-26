using ygo_geo_api.Models;

namespace ygo_geo_api.ViewModels
{
    public class AuthViewModel
    {
        public AuthViewModel(User user)
        {
            this.Username = user.Username;
            this.Role = user.Role;
        }

        public string Username { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}