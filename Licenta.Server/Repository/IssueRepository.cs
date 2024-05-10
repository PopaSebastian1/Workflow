﻿using Licenta.Server.DataLayer.Dto;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using Microsoft.EntityFrameworkCore;

namespace Licenta.Server.Repository
{
    public class IssueRepository
    {
        private readonly AppDbContext _context;
        public IssueRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Issue>> GetAllIssues()
        {
            if (_context.Issues == null)
            {
                return new List<Issue>();
            }
            return await _context.Issues.ToListAsync();
        }
        public async Task<Issue> GetIssueById(Guid id)
        {
            if (_context.Issues == null)
            {
                return null;
            }

            // Verifică dacă issue-ul cu id-ul specificat există
            var issue = await _context.Issues
                .Include(p => p.IssueStatus)
                .Include(p => p.IssueType)
                .Include(p => p.ChildIssues)
                .Include(p => p.Assignee)
                .Include(p => p.Reporter)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (issue == null)
            {
                return null; // Nu există issue cu id-ul specificat
            }

            // Selectează doar id și name din Sprint asociat issue-ului
            var sprintData = await _context.Sprints
                .Where(s => s.Id == issue.SprintId)
                .Select(s => new { s.Id, s.Name })
                .FirstOrDefaultAsync();

            // Actualizează issue-ul cu datele sprint-ului
            issue.Sprint = new Sprint
            {
                Id = sprintData?.Id ?? Guid.Empty, // Utilizează Id-ul sprint-ului sau un Id gol dacă sprintData este null
                Name = sprintData?.Name // Utilizează numele sprint-ului sau null dacă sprintData este null
            };

            return issue;
        }

        public async Task<List<Issue>> GetIssuesByProjectId(Guid projectId)
        {
            if (_context.Issues == null)
            {
                return new List<Issue>();
            }

            return await _context.Issues
                .Include(p=>p.IssueStatus)
                .Include(p=>p.IssueType)
                .Include(p=> p.Assignee)
                .Include(p=> p.Reporter)
                                .Where(p => p.ProjectId == projectId)
                                .ToListAsync();
    }
        public async Task<List<Issue>> GetIssuesBySprintId(Guid sprintId)
        {
            if (_context.Issues == null)
            {
                return new List<Issue>();
            }
            return await _context.Issues.Where(p => p.SprintId == sprintId).ToListAsync();
        }
        public async Task<List<Issue>> GetIssuesByAssigneeId(Guid assigneeId)
        {
            if (_context.Issues == null)
            {
                return new List<Issue>();
            }
            return await _context.Issues.Where(p => p.AssigneeId == assigneeId).ToListAsync();
        }
        public async Task<List<Issue>> GetIssuesByReporterId(Guid reporterId)
        {
            if (_context.Issues == null)
            {
                return new List<Issue>();
            }
            return await _context.Issues.Where(p => p.ReporterId == reporterId).ToListAsync();
        }
        public async Task<List<Issue>> GetIssuesByIssueStatusId(int issueStatusId)
        {
            if (_context.Issues == null)
            {
                return new List<Issue>();
            }
            return await _context.Issues.Where(p => p.IssueStatusId == issueStatusId).ToListAsync();
        }
        public async Task<List<Issue>> GetIssuesByIssueTypeId(int issueTypeId)
        {
            if (_context.Issues == null)
            {
                return new List<Issue>();
            }
            return await _context.Issues.Where(p => p.IssueTypeId == issueTypeId).ToListAsync();
        }
        public async Task<Issue> AddIssue(AddIssueDTO newIssue)
        {
            if(newIssue == null)
            {
                return null;
            }
            //serach if the issueStatus and IssueType exists
            if(await _context.IssuesStatus.Where(p => p.Name == newIssue.IssueStatus).FirstOrDefaultAsync() == null)
            {
                _context.IssuesStatus.Add(new IssueStatus
                {
                    Name = newIssue.IssueStatus
                });
            }
            if(await _context.IssueTypes.Where(p => p.Name == newIssue.IssueType).FirstOrDefaultAsync() == null)
            {
                _context.IssueTypes.Add(new IssueType
                {
                    Name = newIssue.IssueType,
                    IconUrl = "https://www.flaticon.com/svg/static/icons/svg/3523/3523063.svg"
                   

                });
                _context.SaveChanges();
            }
            var issue = new Issue
            {
                Id = Guid.NewGuid(),
                Title = newIssue.Title,
                Description = newIssue.Description,
                CreatedAt = newIssue.CreatedAt,
                DueDate = newIssue.DueDate,
                EstimatedTime = newIssue.EstimatedTime,
                LoggedTime = newIssue.LoggedTime,
                ProjectId = newIssue.ProjectId,
                SprintId = newIssue.SprintId,
                AssigneeId = await _context.Users.Where(p => p.Email == newIssue.AssigneEmail).Select(p => p.Id).FirstOrDefaultAsync(),
                ReporterId = await _context.Users.Where(p => p.Email == newIssue.ReporterEmail).Select(p => p.Id).FirstOrDefaultAsync(),
                IssueStatusId =  await _context.IssuesStatus.Where(p => p.Name == newIssue.IssueStatus).Select(p => p.Id).FirstOrDefaultAsync(),
                IssueTypeId = await _context.IssueTypes.Where(p => p.Name == newIssue.IssueType).Select(p => p.Id).FirstOrDefaultAsync(),
                Project = await _context.Projects.Where(p => p.ProjectId == newIssue.ProjectId).FirstOrDefaultAsync(),
                Sprint = await _context.Sprints.Where(p => p.Id == newIssue.SprintId).FirstOrDefaultAsync(),
                Assignee = await _context.Users.Where(p => p.Email == newIssue.AssigneEmail).FirstOrDefaultAsync(),
                Reporter = await _context.Users.Where(p => p.Email == newIssue.ReporterEmail).FirstOrDefaultAsync(),
                IssueStatus = await _context.IssuesStatus.Where(p => p.Name == newIssue.IssueStatus).FirstOrDefaultAsync(),
                IssueType = await _context.IssueTypes.Where(p => p.Name == newIssue.IssueType).FirstOrDefaultAsync()
            };
            //add the issue to the project too
            var project= await _context.Projects.Where(p => p.ProjectId == newIssue.ProjectId).FirstOrDefaultAsync();
            var sprint= await _context.Sprints.Where(p => p.Id == newIssue.SprintId).FirstOrDefaultAsync();
            if(project.Issues == null)
            {
                project.Issues = new List<Issue>();
            }
            project.Issues.Add(issue);
            if(sprint.Issues == null)
            {
                sprint.Issues = new List<Issue>();
            }
            sprint.Issues.Add(issue);
            _context.Projects.Update(project);
            _context.Sprints.Update(sprint);
            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();
            return issue;
            }
        public async Task DeleteIssue(Guid id)
        {
            var issue = await _context.Issues.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issue != null)
            {
                _context.Issues.Remove(issue);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteIssueByTitle(string title)
        {
            var issue = await _context.Issues.Where(p => p.Title == title).FirstOrDefaultAsync();
            if (issue != null)
            {
                _context.Issues.Remove(issue);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateIssue(AddIssueDTO updatedIssue)
        {
            var assigneeId = await _context.Users.Where(p => p.Email == updatedIssue.AssigneEmail).Select(p => p.Id).FirstOrDefaultAsync();
            var issue= await _context.Issues.Where(p => p.Id == updatedIssue.Id).FirstOrDefaultAsync();
            if (issue != null)
            {
                issue.Title = updatedIssue.Title;
                issue.Description = updatedIssue.Description;
                issue.CreatedAt = updatedIssue.CreatedAt;
                issue.DueDate = updatedIssue.DueDate;
                issue.EstimatedTime = updatedIssue.EstimatedTime;
                issue.LoggedTime = updatedIssue.LoggedTime;
                issue.ProjectId = updatedIssue.ProjectId;
                issue.SprintId = updatedIssue.SprintId;
                issue.AssigneeId = await _context.Users.Where(p => p.Email == updatedIssue.AssigneEmail).Select(p => p.Id).FirstOrDefaultAsync();
                issue.ReporterId = await _context.Users.Where(p => p.Email == updatedIssue.ReporterEmail).Select(p => p.Id).FirstOrDefaultAsync();
                issue.IssueStatusId = await _context.IssuesStatus.Where(p => p.Name == updatedIssue.IssueStatus).Select(p => p.Id).FirstOrDefaultAsync();
                issue.IssueTypeId = await _context.IssueTypes.Where(p => p.Name == updatedIssue.IssueType).Select(p => p.Id).FirstOrDefaultAsync();
                issue.Project = await _context.Projects.Where(p => p.ProjectId == updatedIssue.ProjectId).FirstOrDefaultAsync();
                issue.Sprint = await _context.Sprints.Where(p => p.Id == updatedIssue.SprintId).FirstOrDefaultAsync();
                issue.Assignee = await _context.Users.Where(p => p.Email == updatedIssue.AssigneEmail).FirstOrDefaultAsync();
                issue.Reporter = await _context.Users.Where(p => p.Email == updatedIssue.ReporterEmail).FirstOrDefaultAsync();
                issue.IssueStatus = await _context.IssuesStatus.Where(p => p.Name == updatedIssue.IssueStatus).FirstOrDefaultAsync();
                issue.IssueType = await _context.IssueTypes.Where(p => p.Name == updatedIssue.IssueType).FirstOrDefaultAsync();
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateIssueTitle(Guid id, string newTitle)
        {
            var issue = await _context.Issues.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issue != null)
            {
                issue.Title = newTitle;
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateIssueDescription(Guid id, string newDescription)
        {
            var issue = await _context.Issues.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issue != null)
            {
                issue.Description = newDescription;
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateIssueDueDate(Guid id, DateTime newDueDate)
        {
            var issue = await _context.Issues.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issue != null)
            {
                issue.DueDate = newDueDate;
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateIssueEstimatedTime(Guid id, float newEstimatedTime)
        {
            var issue = await _context.Issues.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issue != null)
            {
                issue.EstimatedTime = newEstimatedTime;
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateIssueLoggedTime(Guid id, float newLoggedTime)
        {
            var issue = await _context.Issues.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issue != null)
            {
                issue.LoggedTime = newLoggedTime;
                await _context.SaveChangesAsync();
            }
        }
        public async Task AddIssueLoggedTime(Guid id, float newLoggedTime)
        {
            var issue = await _context.Issues.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issue != null)
            {
                issue.LoggedTime += newLoggedTime;
                await _context.SaveChangesAsync();
            }
        }
        public async Task RemoveIssueLoggedTime(Guid id, float newLoggedTime)
        {
            var issue = await _context.Issues.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issue != null)
            {
                issue.LoggedTime -= newLoggedTime;
                await _context.SaveChangesAsync();
            }
        }
        public async Task ChangeAssigne(Guid id, Guid newAssigneeId)    
        {
            var issue = await _context.Issues.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issue != null)
            {
                issue.AssigneeId = newAssigneeId;
                issue.Assignee = await _context.Users.Where(p => p.Id == newAssigneeId).FirstOrDefaultAsync();
                await _context.SaveChangesAsync();
            }
        }
        public async Task ChangeReporter(Guid id, Guid newReporterId)
        {
            var issue = await _context.Issues.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issue != null)
            {
                issue.ReporterId = newReporterId;
                issue.Reporter = await _context.Users.Where(p => p.Id == newReporterId).FirstOrDefaultAsync();
                await _context.SaveChangesAsync();
            }
        }
        public async Task ChangeIssueStatus(Guid id, int newIssueStatusId)
        {
            var issue = await _context.Issues.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issue != null)
            {
                issue.IssueStatusId = newIssueStatusId;
                issue.IssueStatus = await _context.IssuesStatus.Where(p => p.Id == newIssueStatusId).FirstOrDefaultAsync();
                await _context.SaveChangesAsync();
            }
        }
        public async Task ChangeIssueType(Guid id, int newIssueTypeId)
        {
            var issue = await _context.Issues.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (issue != null)
            {
                issue.IssueTypeId = newIssueTypeId;
                issue.IssueType = await _context.IssueTypes.Where(p => p.Id == newIssueTypeId).FirstOrDefaultAsync();
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Issue>> GetIssuesByProjectIdAndSprintId(Guid projectId, Guid sprintId)
        {
            if (_context.Issues == null)
            {
                return new List<Issue>();
            }
            return await _context.Issues.Where(p => p.ProjectId == projectId && p.SprintId == sprintId).ToListAsync();
        }
        public async Task<List<Issue>> GetAllChildIssues(Guid parentId)
        {
            var parentIssue = await _context.Issues.Include(p=>p.ChildIssues).Where(p => p.Id == parentId).FirstOrDefaultAsync();
            if (parentIssue == null)
            {
                return new List<Issue>();
            }
            return parentIssue.ChildIssues;
        }
        public async Task<Issue> GetParentIssue(Guid childId)
        {
            var childIssue = await _context.Issues.Where(p => p.Id == childId).FirstOrDefaultAsync();
            if (childIssue == null)
            {
                return null;
            }
            return childIssue.ParentIssue;
        }
        public async Task AddChildIssue(Guid parentId, Guid childId)
        {
            var parentIssue = await _context.Issues.Include(p=>p.ChildIssues).Where(p => p.Id == parentId).FirstOrDefaultAsync();
            var childIssue = await _context.Issues.Where(p => p.Id == childId).FirstOrDefaultAsync();
            if (parentIssue != null && childIssue != null)
            {
                parentIssue.ChildIssues.Add(childIssue);
                childIssue.ParentIssue = parentIssue;
                childIssue.ParentIssueId = parentId;
                await _context.SaveChangesAsync();
            }
        }
        public async Task RemoveChildIssue(Guid parentId, Guid childId)
        {
            var parentIssue = await _context.Issues.Where(p => p.Id == parentId).FirstOrDefaultAsync();
            var childIssue = await _context.Issues.Where(p => p.Id == childId).FirstOrDefaultAsync();
            if (parentIssue != null && childIssue != null)
            {
                parentIssue.ChildIssues.Remove(childIssue);
                childIssue.ParentIssue = null;
                childIssue.ParentIssueId = null;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Issue>> GetAllChildIssueByParentId(Guid parentId)
        {
            List<Issue> childIssues = new List<Issue>();
            childIssues = await _context.Issues.Where(p => p.ParentIssueId == parentId).ToListAsync();
            if(childIssues == null)
            {
                return new List<Issue>();
            }
            return childIssues;
        }
    }
}
