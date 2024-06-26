﻿using Licenta.Server.DataLayer.DataBase;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.Repository;

namespace Licenta.Server.DataLayer.Utils
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context, UserRepository userRepository, UserSkillRepository userSkillRepository, SkillRepository skillRepository, SprintRepository sprintRepository, ProjectRepository projectsRepository, IssueStatusRepository issueStatusRepository, IssueTypeRepository issueTypeRepository, IssueRepository issueRepository, CommentRepository commentRepository)
        {
            _context = context;
            UserRepository = userRepository;
            UserSkillRepository = userSkillRepository;
            SkillRepository = skillRepository;
            SprintRepository = sprintRepository;
            ProjectsRepository = projectsRepository;
            IssueStatusRepository = issueStatusRepository;
            IssueTypeRepository = issueTypeRepository;
            IssueRepository = issueRepository;
            CommentRepository = commentRepository;
        }
        public UserRepository UserRepository { get; }
        public UserSkillRepository UserSkillRepository { get; }
        public SkillRepository SkillRepository { get; }
        public IssueStatusRepository IssueStatusRepository { get; }
        public IssueRepository IssueRepository { get; }
        public IssueTypeRepository IssueTypeRepository { get; }
        public SprintRepository SprintRepository { get; }
        public ProjectRepository ProjectsRepository { get; }
        public CommentRepository CommentRepository { get; }
        public IssueLabelRepository IssueLabelRepository { get; }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var errorMessage = "Error when saving to the database: "
                               + $"{ex.Message}\n\n"
                               + $"{ex.InnerException}\n\n"
                               + $"{ex.StackTrace}\n\n";

                Console.WriteLine(errorMessage);

            }
        }
    }
}
