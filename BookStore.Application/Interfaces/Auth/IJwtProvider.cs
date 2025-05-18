using BookStore.Core.Models;

namespace BooksStore.Infrastructure;

public interface IJwtProvider
{
    string GenerateToken(User user);
}