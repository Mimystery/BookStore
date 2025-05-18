using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Contracts.Users
{
    public record RegisterUserRequest(
        [Required] string UserName,
        [Required] string Password,
        [Required] string Email);
}
