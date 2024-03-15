using Licenta.Server.DataLayer.Dto;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using Microsoft.EntityFrameworkCore;

namespace Licenta.Server.Repository
{
    public class ProjectRepository
    {
        private readonly AppDbContext _context;
        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Project>> GetAllProjects()
        {
            if (_context.Projects == null)
            {
                return new List<Project>();
            }
            return await _context.Projects.ToListAsync();
        }
        public async Task<Project> GetProjectById(Guid id)
        {
            if (_context.Projects == null)
            {
                return null;
            }
            if (_context.Projects.Where(p => p.ProjectId == id) == null)
            {
                return null;
            }
            return await _context.Projects.Where(p => p.ProjectId == id).FirstOrDefaultAsync();
        }
        public async Task AddProject(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }
        public async Task<Project?> DeleteProject(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            _context.Projects.Remove(project);
            return project;
        }
        public async Task UpdateProject(Project project)
        {
            _context.Entry(project).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<List<Project>> GetAllProjectsForOwner(string email)
        {
            if (_context.Projects == null)
            {
                return new List<Project>();
            }
            return await _context.Projects.Where(p => p.Owner.Email == email).ToListAsync();
        }
        public async Task AddMemberToProject(string email, Guid projectId)
        {
            var user = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            var project = await _context.Projects.Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            project.Members.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveMemberFromProject(string email, Guid projectId)
        {
            var user = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            var project = await _context.Projects.Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            project.Members.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllUsersForProject(Guid id)
        {
            var project = await _context.Projects.Where(p => p.ProjectId == id).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            return project.Members;
        }
        //add a new project into the database
        public async Task<Project> AddProject(CreateProjectDTO newProject, Guid ownerId)
        {
            var user=await _context.Users.Where(p => p.Id == ownerId).FirstOrDefaultAsync();
            var project = new Project
            {
                ProjectId = Guid.NewGuid(),
                Name = newProject.Name,
                Description = newProject.Description,
                StartDate = newProject.StartDate,
                EndDate = newProject.EndDate,
                OwnerId = ownerId,
                Owner = await _context.Users.Where(p => p.Id == ownerId).FirstOrDefaultAsync(),
                Members = new List<User>()

            };
            project.Members.Add(user);
            List<Sprint> sprints= new List<Sprint>();
            //add a new sprint into the database
            var sprint=new Sprint
            {
                Id = Guid.NewGuid(),
                Name = "Sprint 1",
                StartDate = newProject.StartDate,
                EndDate = newProject.StartDate.AddDays(14),
                ProjectId = project.ProjectId,
                Project = project,
                Issues = new List<Issue>()
            };
            project.Sprints.Add(sprint);
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }
        public async Task<List<Sprint>> GetAllSprintsForProject(Guid id)
        {
            var project = await _context.Projects.Where(p => p.ProjectId == id).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            return project.Sprints;
        }
        public async Task<List<Issue>> GetAllIssuesForProject(Guid id)
        {
            var project = await _context.Projects.Where(p => p.ProjectId == id).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            return project.Issues;
        }
        public async Task<List<Issue>> GetAllIssuesForSprint(Guid id)
        {
            var sprint = await _context.Sprints.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (sprint == null)
            {
                throw new ArgumentNullException(nameof(sprint));
            }
            return sprint.Issues;
        }
        public async Task<List<Issue>> GetAllIssuesForProjectAndSprint(Guid projectId, Guid sprintId)
        {
            var project = await _context.Projects.Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            var sprint = await _context.Sprints.Where(p => p.Id == sprintId).FirstOrDefaultAsync();
            if (sprint == null)
            {
                throw new ArgumentNullException(nameof(sprint));
            }
            return sprint.Issues;
        }

    }
}
