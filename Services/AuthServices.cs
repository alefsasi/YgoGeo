using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ygo_geo_api.Models;
using ygo_geo_api.ViewModels;

namespace ygo_geo_api.Services
{
    public class AuthServices
    {
        private readonly IOptions<AppSettings> appSettings;
        private readonly IPasswordHasher _passwordHasher;
        private readonly UserServices _userServices;
        public AuthServices(IOptions<AppSettings> app, IPasswordHasher passwordHasher, UserServices userServices)
        {
            this.appSettings = app;
            _passwordHasher = passwordHasher;
            _userServices = userServices;

        }
        public void Signup(User user)
        {

            var usuarioExiste = _userServices.GetUserByName(user.Username);

            if(usuarioExiste != null) {
                throw new Exception("Usuário já cadastrado!");
            }
            
            user.Password = _passwordHasher.Hash(user.Password);
            _userServices.Cadastrar(user);
        }
        public AuthViewModel Authenticate(User user)
        {
            var userLogin = _userServices.GetUserByName(user.Username);
            if (userLogin == null)
                return null;

            var Verificado = _passwordHasher.Check(userLogin.Password, user.Password);

            if (!Verificado)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Value.SecreteKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                new Claim (ClaimTypes.Name, userLogin.Username),
                new Claim ("Store", userLogin.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var authViewModel = new AuthViewModel(userLogin);
            authViewModel.Token = tokenHandler.WriteToken(token);

            return authViewModel;
        }

    }
}