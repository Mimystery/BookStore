using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Core.Abstactions;
using BookStore.Core.Models;
using BookStrore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStrore.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly BookStoreDbContext _dbContext;
        private readonly IMapper _mapper;

        public UsersRepository(BookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Add(User user)
        {
            var userEntity = new UserEntity
            {
                Id = user.Id,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                Email = user.Email
            };

            await _dbContext.Users.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            var userEntity = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception("User not found");

            return _mapper.Map<User>(userEntity);
        }
    }
}
