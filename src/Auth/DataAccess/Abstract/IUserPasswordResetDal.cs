using Core.DataAccess.Abstract;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IUserPasswordResetDal : IEntityRepository<UserPasswordReset>
    {
        Task<UserPasswordReset> GetLastRequestAsync(Guid userId);
    }
}
