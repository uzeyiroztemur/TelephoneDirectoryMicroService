using Core.Entities.DTOs;
using Entities.DTOs.Params;

namespace Business.Abstract
{
    public interface IUserLoginService
    {
        IResult Log(DetailForLoginDTO detailForLoginDTO);
    }
}
