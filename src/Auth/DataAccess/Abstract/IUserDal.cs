using Core.DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Params;
using Entities.DTOs.Results;

namespace DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        UserForViewDTO GetByUserName(string userName);
        UserForUpsertDTO GetForUpsert(Guid id, Guid currentUserId);
    }
}