using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Core.Models
{
    public class User 
    {
        public User()
        {
            
        }
        private User(Guid id, string userName, string password, string email)
        {
            Id = id;
            UserName = userName;
            PasswordHash = password;
            Email = email;
        }

        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        public static User Create(Guid id, string userName, string password, string email)
        {
            return new User(id, userName, password, email);
        }
    }
}
