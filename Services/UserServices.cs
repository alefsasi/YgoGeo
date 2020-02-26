using System.Collections.Generic;
using yu_geo_api.Models;
using yu_geo_api.Repository;

namespace yu_geo_api.Services
{
    public class UserServices
    {
        private readonly UserRepository _userRepository;
        public UserServices(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetUsuarios()
        {
            return _userRepository.GetUsuarios();
        }

        public User GetUserByName(string username)
        {
            return _userRepository.GetUserByName(username);
        }
        public void Cadastrar(User user)
        {
            _userRepository.Cadastrar(user);
        }

    }
}