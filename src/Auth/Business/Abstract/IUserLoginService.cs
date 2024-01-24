using Core.Entities.DTOs;
using Entities.DTOs.Params;

namespace Business.Abstract
{
    public interface IUserLoginService
    {
        Task<IResult> LogAsync(DetailForLoginDTO detailForLoginDTO);
    }
}
