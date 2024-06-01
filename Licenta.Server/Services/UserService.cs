using Licenta.Server.DataLayer.Dto;
using Licenta.Server.DataLayer.Models;
using Licenta.Server.DataLayer.Utils;
using Licenta.Server.Repository.Interfeces;
using Licenta.Server.Services.Interfaces;

namespace Licenta.Server.Services
{
    public class UserService : IUserService
    {
        private readonly UnitOfWork _unitOfWork;
        public UserService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<UserDTO>> GetAllAsync()
        {
            var users = await _unitOfWork.UserRepository.GetUsersAsync();
            List<UserDTO?> usersDTO = new List<UserDTO?>();
            //transform to DTO
            return users;
        }
        public async Task<UserDTO> GetByIdAsync(Guid id)
        {
        //    User user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            
        //    return new UserDTO()
        //    {
        //        Id = user.Id,
        //        Email = user.Email,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        Role = user.Role
        //    };
        return new UserDTO();
        }

        public async Task<User?> DeleteUserAsync(Guid id)
        {
            //return await _unitOfWork.UserRepository.DeleteAsync(id);
            return new User();
        }
        public async Task<User?> AddUserSkillAsync(string email, Skill skill)
        {
            return await _unitOfWork.UserRepository.AddSkillUserAsync(email, skill);
       
        }
        public async Task<UserDTO?> GetUserAsync(string email)
        {
            User user= await _unitOfWork.UserRepository.GetUserAsync(email);
            if(user==null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            UserDTO userDTO = new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                ProfilePicture = user.ProfilePicture,
                Skills = _unitOfWork.UserSkillRepository.GetSkillsAsync(email).Result
            };
            return userDTO;
        }
        public async Task<UserDTO?> GetUserAsyncById(Guid id)
        {
                     User user = await _unitOfWork.UserRepository.GetUserAsyncById(id);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            UserDTO userDTO = new UserDTO()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                ProfilePicture = user.ProfilePicture,
                Skills = _unitOfWork.UserSkillRepository.GetSkillsAsync(user.Email).Result
            };
            return userDTO;
        }
        public async Task<User?> UpdateUserAsync(string email, string firstName, string lastName)
        {
            return await _unitOfWork.UserRepository.UpdateUser(email, firstName, lastName);
        }
        public async Task<Project?> JoinProject(string email, string key)
        {

            return await _unitOfWork.UserRepository.JoinProject(email, key);
        }
        public async Task<List<Project>> GetAllUserProjects(string email)
        {
            return await _unitOfWork.UserRepository.GetAllUserProjects(email);
        }
        public async Task<List<Issue>> GetAllUserIssues(Guid id)
        {
            return await _unitOfWork.UserRepository.GetAllUserIssues(id);
        }
        public async Task<List<Issue>> GetAllUserIssuesByProjectId(Guid projectId, string email)
        {
            return await _unitOfWork.UserRepository.GetAllUserIssuesByProjectId(projectId, email);
        }
    }
}
