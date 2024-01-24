using Core.DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Params;
using Entities.DTOs.Results;

namespace DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        Task<UserForViewDTO> GetByUserNameAsync(string userName);
        Task<UserForUpsertDTO> GetForUpsertAsync(Guid id);
    }
}