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
        return await _context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
    }
    public async Task<User?> GetUserAsyncById(Guid id)
    {
        return await _context.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
    }
    public async Task<User?> UpdateUser(string email, string firstName, string lastName)
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
    public async Task<List<Project>> GetAllUserProjects(string email)
    {
        var user = await _context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return await _context.Projects.Where(p => p.Members.Contains(user)).ToListAsync();
    }
    public async Task<Project?> JoinProject(string email, string projectKey)
    {
        var user = await _context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        var project = await _context.Projects.Where(p => p.Key == projectKey).FirstOrDefaultAsync();
        if (project == null)
        {
            throw new ArgumentNullException(nameof(project));
        }
        project.Members.Add(user);
        _context.SaveChanges();
        return project;
    }
    public async Task<List<Issue>> GetAllUserIssues(Guid id)
    {
        var user = await _context.Users.Where(user => user.Id == id).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return await _context.Issues.Where(i => i.Assignee.Id == id).ToListAsync();
    }
    public async Task<List<Issue>> GetAllUserIssuesByProjectId(Guid projectId, string email)
    {
        var user = await _context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        return await _context.Issues.Where(i => i.Assignee.Email == email && i.Project.ProjectId == projectId).ToListAsync();
    }
    public async Task<List<Project>> GetAllUserProjects(Guid userId)
    {
        var user= await _context.Users.Where(user => user.Id == userId).FirstOrDefaultAsync();
        var projects = await _context.Projects.Where(p => p.Members.Contains(user)).ToListAsync();
        return projects;
    }
}
