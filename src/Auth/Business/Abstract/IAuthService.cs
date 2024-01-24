using Core.Entities.DTOs;
using Core.Utilities.Security;
using Entities.DTOs.Params;
using Entities.DTOs.Results;

namespace Business.Abstract
{
    public interface IAuthService
    {
        Task<IDataResult<UserForViewDTO>> LoginAsync(UserForLoginDTO userForLoginDTO);
        Task<IDataResult<AccessToken>> CreateAccessTokenAsync(UserForViewDTO userForViewDTO);
        Task<IResult> ChangePasswordAsync(PasswordForChangeDTO passwordForChangeDTO);
    }
}
