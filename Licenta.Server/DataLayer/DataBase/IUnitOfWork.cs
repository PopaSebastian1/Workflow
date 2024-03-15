using Licenta.Server.Repository;

namespace Licenta.Server.DataLayer.DataBase
{
    public interface IUnitOfWork
    {
        UserRepository UserRepository { get; }
        UserSkillRepository UserSkillRepository { get; }
    }
}
