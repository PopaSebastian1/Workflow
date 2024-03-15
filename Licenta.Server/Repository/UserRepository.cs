using Licenta.Server.DataLayer.Dto;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using Licenta.Server.Repository.Interfeces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Licenta.Server.Repository;

public class UserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) 
    {
        _context = context;
    }
    public async Task<List<UserDTO>> GetUsersAsync()
    {
        var users = await _context.Users.ToListAsync();
        List<UserDTO?> usersDTO = new List<UserDTO?>();
        //transform to DTO
        return users.Where(u => u != null).Select(u => new UserDTO()
        {
            Id = u.Id,
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Role = u.Role,
            ProfilePicture = u.ProfilePicture,
            

        }).ToList();
    }
    public async Task<User?> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        _context.Users.Remove(user);
        return user;
    }
    public async Task<User?> AddSkillUserAsync(string email, Skill skill)
    {
        var user = await _context.Users.FindAsync(email);
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.Skills.Add(skill);
        return user;
    }
    public async Task<User?> GetUserAsync(string email)
    {
        return await _context.Users.Where(user=>user.Email==email).FirstOrDefaultAsync();
    }
    public async Task<User?>UpdateUser(string email, string firstName, string lastName)
    {
        var user = await _context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        user.FirstName = firstName;
        user.LastName = lastName;
        //user.ProfilePicture = profilePicture;
        return user;
    }
    public async Task<List<Project>>GetAllUserProjects(string email)
    {
        var user = await _context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return await _context.Projects.Where(p => p.Owner.Email == email).ToListAsync();
    }
}
