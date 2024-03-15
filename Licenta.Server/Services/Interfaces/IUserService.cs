using Licenta.Server.DataLayer.Dto;
using Licenta.Server.DataLayer.Models;

namespace Licenta.Server.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDTO>> GetAllAsync();
    Task<UserDTO?> GetByIdAsync(Guid id);

    Task<UserDTO?> GetUserAsync(string email);
    Task<User?> DeleteUserAsync(Guid id);

}
