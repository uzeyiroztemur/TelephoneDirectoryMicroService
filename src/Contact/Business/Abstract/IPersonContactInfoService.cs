using Core.Entities.DTOs;
using Entities.DTOs.Params;

namespace Business.Abstract
{
    public interface IPersonContactInfoService
    {
        IResult Add(Guid personId, PersonContactInfoForUpsertDTO model);
        IResult Update(Guid personId, Guid id, PersonContactInfoForUpsertDTO model);
        IResult Delete(Guid personId, Guid id);
    }
}
