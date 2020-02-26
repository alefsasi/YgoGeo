using System;
using System.Collections.Generic;
using System.Linq;
using yu_geo_api.Models;

namespace yu_geo_api.Repository
{
    public class UserRepository
    {
        private readonly Contexto _db;

        public UserRepository(Contexto db)
        {
            _db = db;
        }

        public List<User> GetUsuarios()
        {
            return _db.Usuarios.ToList();
        }
        public User GetUserByName(string username)
        {
            return _db.Usuarios.Where(x => x.Username == username).FirstOrDefault();
        }
        public void Cadastrar(User user)
        {
            try
            {
                _db.Usuarios.Add(user);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw ex;
            }

        }
    }
}