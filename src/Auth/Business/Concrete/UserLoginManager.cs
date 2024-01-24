using Business.Abstract;
using Business.Constants;
using Core.Entities.DTOs;
using Core.Extensions;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Params;
using UAParser;

namespace Business.Concrete
{
    public class UserLoginManager : BaseManager, IUserLoginService
    {
        private readonly IUserLoginDal _userLoginDal;

        public UserLoginManager(IUserLoginDal userLoginDal)
        {
            _userLoginDal = userLoginDal;
        }

        #region Helpers
        private ClientInfo ParseUserAgent(string userAgent)
        {
            var uaParser = Parser.GetDefault();
            return uaParser.Parse(userAgent);
        }

        private string GetVersion(string[] versions)
        {
            return string.Join(".", versions);
        }
        #endregion

        public async Task<IResult> LogAsync(DetailForLoginDTO detailForLoginDTO)
        {
            if (detailForLoginDTO.UserAgent.IsEmpty())
                return new ErrorResult(Messages.UserAgentNotFound);

            var clientInfo = ParseUserAgent(detailForLoginDTO.UserAgent);
            if (clientInfo.IsNull())
                return new ErrorResult(Messages.UserAgentParseError);

            var entityToAdd = new UserLogin
            {
                UserId = detailForLoginDTO.UserId,
                CreatedBy = detailForLoginDTO.UserId,
                DeviceId = detailForLoginDTO.UserDeviceId,

                UserAgentText = detailForLoginDTO.UserAgent,
                IpAddress = detailForLoginDTO.IpAddress,

                UserAgent = clientInfo.UA.Family.NotEmpty() ? clientInfo.UA.Family : null,
                UserAgentVersion = GetVersion(new[] { clientInfo.UA?.Major ?? "0", clientInfo.UA?.Minor ?? "0" }) ?? null,
            };

            await _userLoginDal.AddAsync(entityToAdd);

            return new SuccessResult();
        }
    }
}
