using Licenta.Server.DataLayer.Dto;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Security.Policy;
using RandomStringUtils;
using System.Reflection.Emit;
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
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var projects = await _context.Projects
                .Include(p => p.Members)
                .Include(p => p.Sprints)
                    .ThenInclude(s => s.Issues)
                .Select(p => new Project
                {
                    ProjectId = p.ProjectId,
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    OwnerId = p.OwnerId,
                    Owner = p.Owner,
                    Members = p.Members,
                    Sprints = p.Sprints,
                    Issues = p.Issues
                })
                .ToListAsync();

            return projects;
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
            return await _context.Projects.Include(p=>p.Members)
                .Where(p => p.ProjectId == id).FirstOrDefaultAsync();
        }
        public async Task<Project?> DeleteProject(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            _context.Projects.Remove(project);
            _context.SaveChangesAsync();
            return project;
        }
        public async Task UpdateProject(AddProjectDto project)
        {
            Project updateProject = await _context.Projects.Where(p => p.ProjectId == project.ProjectId).FirstOrDefaultAsync();
            if (updateProject == null)
            {
                throw new ArgumentNullException(nameof(updateProject));
            }
            updateProject.Name = project.Name;
            updateProject.Description = project.Description;
            updateProject.StartDate = project.StartDate;
            updateProject.EndDate = project.EndDate;
            updateProject.OwnerId = project.OwnerId;
            updateProject.Owner = await _context.Users.Where(p => p.Id == project.OwnerId).FirstOrDefaultAsync();

            _context.Projects.Update(updateProject);
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
        public async Task<List<Project>> GetAllProjectsForMemeber(string email)
        {
            if (_context.Projects == null)
            {
                return new List<Project>();
            }
            return await _context.Projects.Where(p => p.Members.Any(u => u.Email == email)).ToListAsync();
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
            //_context.Projects.Where(p => p.ProjectId == projectId).FirstOrDefault().Members.Add(user);
            project.Members.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveMemberFromProject(string email, Guid projectId)
        {
            var user = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
            var project = await _context.Projects
        .Include(p => p.Issues)
            .ThenInclude(i => i.Assignee)
        .Include(p => p.Issues)
            .ThenInclude(i => i.Reporter)
        .Include(p=>p.Members)
        .Include(p=>p.Administrators)
        .Include(p=>p.Owner)
        .Where(p => p.ProjectId == projectId)
        .FirstOrDefaultAsync();
            if(project.OwnerId==user.Id)
            {
                throw new Exception("The owner of the project cannot be removed from the project");
            }
            if(!project.Administrators.Contains(user))
            {
                throw new Exception("The user is not an adminstrator of the project");
            }
           
            var owner = await _context.Users.Where(u => u.Id == project.OwnerId).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            project.Issues.Where(i => i.AssigneeId == user.Id || i.ReporterId == user.Id).ToList().ForEach(i =>
            {
                if (i.AssigneeId == user.Id)
                {
                    i.Assignee = owner;
                    i.AssigneeId = owner.Id;
                }
                if (i.ReporterId == user.Id)
                {
                    i.Reporter = owner;
                    i.ReporterId = owner.Id;
                }
            });
            project.Members.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllUsersForProject(Guid id)
        {
            var project = await _context.Projects.Include(p => p.Members).Where(p => p.ProjectId == id).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
           return project.Members;
        }
        public async Task<Project> AddProject(CreateProjectDTO newProject, Guid ownerId)
        {
            var user = await _context.Users.Where(p => p.Id == ownerId).FirstOrDefaultAsync();
            string key=RandomStringUtils.RandomStringUtils.RandomAlphanumeric(12);

            var project = new Project
            {
                ProjectId = Guid.NewGuid(),
                Name = newProject.Name,
                Description = newProject.Description,
                StartDate = newProject.StartDate.ToUniversalTime(),
                EndDate = newProject.EndDate.ToUniversalTime(),
                OwnerId = ownerId,
                Owner = user,
                Key = key,
                Administrators = new List<User>()
                
                
            };
            project.Owner = user;
            project.Administrators.Add(user);
            List<Sprint> sprints = new List<Sprint>();
            List<User> members = new List<User>();
            members.Add(user);
            project.Members = members;
            //add a new sprint into the database
            var sprint = new Sprint
            {
                Id = Guid.NewGuid(),
                Name = "Sprint 1",
                StartDate = newProject.StartDate,
                EndDate = newProject.StartDate.AddDays(14),
                ProjectId = project.ProjectId,
                Project = project,
                Issues = new List<Issue>(),
                
            };
            List<IssueStatus> issueStatus =
            [
                _context.IssuesStatus.Where(p => p.Id == 1).FirstOrDefault(),
                _context.IssuesStatus.Where(p => p.Id == 2).FirstOrDefault(),
                _context.IssuesStatus.Where(p => p.Id == 3).FirstOrDefault(),
            ];
            project.IssueStatuses=issueStatus;
            project.Sprints.Add(sprint);
            _context.Projects.Add(project);
            _context.Sprints.Add(sprint);
            await _context.SaveChangesAsync();
            return project;
        }
        public async Task<List<Sprint>> GetAllSprintsForProject(Guid id)
        {
            // Obține proiectul cu sprints asociate
            var project = await _context.Projects
                .Where(p => p.ProjectId == id)
                .Include(p => p.Sprints)
                .FirstOrDefaultAsync();

            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            // Selectează doar id și name pentru fiecare sprint asociat proiectului
            var sprints = project.Sprints.Select(s => new Sprint
            {
                Id = s.Id,
                Name = s.Name,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                
                // Alte proprietăți de copiat aici, dacă există
            }).ToList();

            return sprints;
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
        public async Task AddIssueStatusToProject(Guid projectId, int issueStatusId)
        {
            var project = await _context.Projects.Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            var issueStatus = await _context.IssuesStatus.Where(p => p.Id == issueStatusId).FirstOrDefaultAsync();
            if (issueStatus == null)
            {
                throw new ArgumentNullException(nameof(issueStatus));
            }
            project.IssueStatuses.Add(issueStatus);
            issueStatus.Projects.Add(project);
            await _context.SaveChangesAsync();
        }
        public async Task AddNewIssueStatusToProject(Guid projectId, IssueStatus issueStatus)
        {
            var project = await _context.Projects.Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            project.IssueStatuses.Add(issueStatus);
            issueStatus.Projects.Add(project);
            await _context.SaveChangesAsync();
        }
        public async Task<List<IssueStatus>> GetIssueStatusFromProject(Guid projectId)
        {
            return await _context.Projects.Where(p => p.ProjectId == projectId).Select(p => p.IssueStatuses).FirstOrDefaultAsync();
        }
        public async Task RemoveIssueStatusFromProject(Guid projectId, int issueStatusId)
        {
            var project = await _context.Projects.Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            var issueStatus = await _context.IssuesStatus.Where(p => p.Id == issueStatusId).FirstOrDefaultAsync();
            if (issueStatus == null)
            {
                throw new ArgumentNullException(nameof(issueStatus));
            }
            project.IssueStatuses.Remove(issueStatus);
            issueStatus.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllAdministratorsForProject(Guid projectId)
        {
            var project = await _context.Projects.Include(p=>p.Administrators).Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            return project.Administrators;
        }
        public async Task AddAdministratorToProject(Guid projectId, string email)
        {
            var user = await _context.Users.Include(p=>p.AdministratorProjects).Where(u => u.Email == email).FirstOrDefaultAsync();
            var project = await _context.Projects.Include(p=>p.Administrators).Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            user.AdministratorProjects.Add(project);
            project.Administrators.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveAdministratorFromProject(Guid projectId, string email)
        {
            var user = await _context.Users.Include(p => p.AdministratorProjects).Where(u => u.Email == email).FirstOrDefaultAsync();
            var project = await _context.Projects.Include(p => p.Administrators).Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            user.AdministratorProjects.Remove(project);
            project.Administrators.Remove(user);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveSprintFromProject(Guid projectId, Guid sprintId)
        {
            var project = await _context.Projects.Include(p => p.Sprints).Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            var sprint = await _context.Sprints.Include(p=>p.Issues).Where(p => p.Id == sprintId).FirstOrDefaultAsync();
            if (sprint == null)
            {
                throw new ArgumentNullException(nameof(sprint));
            }
            var issues = sprint.Issues;
            var sprints = project.Sprints;
            //get the next sprint or the previous one
            Sprint newSprint;
            if(sprints.Count>1)
            {
               int index = sprints.IndexOf(sprint);
                if(sprints.Count>index+1)
                {
                    newSprint = sprints[index + 1];
                }
                else
                {
                    newSprint = sprints[index - 1];
                }
                foreach(var issue in issues)
                {
                    issue.Sprint = newSprint;
                    issue.SprintId = newSprint.Id;
                }
            }
            project.Sprints.Remove(sprint);
            await _context.SaveChangesAsync();
        }
        public async Task AddIssueLabel(Guid projectId, IssueLabel label)
        {
            var project = await _context.Projects.Include(p => p.IssueLabels).Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            label.Id = 0;
            project.IssueLabels.Add(label);
            label.ProjectId = projectId;
            _context.Labels.Add(label);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveIssueLabel(Guid projectId, int labelId)
        {
            var project = await _context.Projects.Include(p => p.IssueLabels).Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            var label = await _context.Labels.Where(p => p.Id == labelId).FirstOrDefaultAsync();
            if (label == null)
            {
                throw new ArgumentNullException(nameof(label));
            }
            project.IssueLabels.Remove(label);
            label.ProjectId = Guid.Empty;
            _context.Labels.Remove(label);
            await _context.SaveChangesAsync();
        }
        public async Task<List<IssueLabel>> GetAllLabelsForProject(Guid projectId)
        {
            return await _context.Projects.Include(p => p.IssueLabels).Where(p => p.ProjectId == projectId).Select(p => p.IssueLabels).FirstOrDefaultAsync();
        }
        public async Task RemoveIssueFromProject(Guid projectId, Guid issueId)
        {
            var project = await _context.Projects.Include(p => p.Issues).Where(p => p.ProjectId == projectId).FirstOrDefaultAsync();
            var issue = await _context.Issues.Where(p => p.Id == issueId).FirstOrDefaultAsync();
            if(project==null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            if(issue==null)
            {
                throw new ArgumentNullException(nameof(issue));
            }
            project.Issues.Remove(issue);
            _context.Issues.Remove(issue);
            _context.SaveChangesAsync();
        }
    }
}
