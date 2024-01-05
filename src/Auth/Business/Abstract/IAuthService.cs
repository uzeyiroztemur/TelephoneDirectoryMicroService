using Core.Entities.DTOs;
using Core.Utilities.Security;
using Entities.DTOs.Params;
using Entities.DTOs.Results;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<UserForViewDTO> Login(UserForLoginDTO userForLoginDTO);
        IDataResult<AccessToken> CreateAccessToken(UserForViewDTO userForViewDTO);
        IResult ChangePassword(PasswordForChangeDTO passwordForChangeDTO);
    }
}
