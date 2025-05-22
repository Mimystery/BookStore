using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Core.Enums;
using BookStore.Core.Models;

namespace BookStore.Core.Abstactions
{
    public interface IUsersRepository
    {
        Task Add(User user);
        Task<User> GetByEmail(string email);
        public Task<HashSet<Permission>> GetUserPermissions(Guid userId);
    }
}
