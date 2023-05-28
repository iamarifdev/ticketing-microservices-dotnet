using Ticketing.Auth.Domain.Entities;

namespace Ticketing.Auth.Persistence.Repositories;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetById(int id);
}