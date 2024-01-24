using Core.Entities.DTOs;
using Entities.DTOs.Params;

namespace Business.Abstract
{
    public interface IPersonContactInfoService
    {
        Task<IResult> AddAsync(Guid personId, PersonContactInfoForUpsertDTO model);
        Task<IResult> UpdateAsync(Guid personId, Guid id, PersonContactInfoForUpsertDTO model);
        Task<IResult> DeleteAsync(Guid personId, Guid id);
    }
}
