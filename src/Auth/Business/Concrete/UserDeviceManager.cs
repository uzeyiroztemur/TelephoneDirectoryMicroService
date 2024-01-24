using Business.Abstract;
using Core.Entities.DTOs;
using Core.Extensions;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Params;

namespace Business.Concrete
{
    public class UserDeviceManager : BaseManager, IUserDeviceService
    {
        private readonly IUserDeviceDal _userDeviceDal;

        public UserDeviceManager(IUserDeviceDal userDeviceDal)
        {
            _userDeviceDal = userDeviceDal;
        }

        public async Task<IDataResult<UserDevice>> UpsertAsync(DeviceForLoginDTO deviceForLoginDTO, string ipAddress, string browserName)
        {
            UserDevice deviceInfo = null;

            if (deviceForLoginDTO.NotificationToken.NotEmpty() || deviceForLoginDTO.DeviceId.NotEmpty())
            {
                if (deviceForLoginDTO.DeviceId.NotEmpty())
                    deviceInfo = await _userDeviceDal.GetAsync(g => g.UserId == deviceForLoginDTO.UserId && g.OS == deviceForLoginDTO.OS && g.DeviceId == deviceForLoginDTO.DeviceId);

                deviceInfo ??= await _userDeviceDal.GetAsync(g => g.UserId == deviceForLoginDTO.UserId && g.OS == deviceForLoginDTO.OS && g.NotificationToken == deviceForLoginDTO.NotificationToken);
            }
            else
            {
                if (browserName == "Other")
                    deviceInfo = await _userDeviceDal.GetAsync(g => g.UserId == deviceForLoginDTO.UserId && g.OS == deviceForLoginDTO.OS && g.Model == deviceForLoginDTO.Model && g.Name == deviceForLoginDTO.Name && g.BrowserName == browserName);
                else
                    deviceInfo = await _userDeviceDal.GetAsync(g => g.UserId == deviceForLoginDTO.UserId && g.OS == deviceForLoginDTO.OS && g.IpAddress == ipAddress && g.BrowserName == browserName);
            }

            if (deviceInfo.IsNull())
            {
                deviceInfo = new UserDevice
                {
                    UserId = deviceForLoginDTO.UserId,
                    IpAddress = ipAddress,
                    OS = deviceForLoginDTO.OS,
                    OSVersion = deviceForLoginDTO.OSVersion,
                    Model = deviceForLoginDTO.Model,
                    Name = deviceForLoginDTO.Name,
                    BrowserName = browserName,
                    NotificationToken = deviceForLoginDTO.NotificationToken,
                    DeviceId = deviceForLoginDTO.DeviceId,
                    IsActive = true,
                    CreatedBy = deviceForLoginDTO.UserId,
                };

                await _userDeviceDal.AddAsync(deviceInfo);
            }
            else
            {
                deviceInfo.OSVersion = deviceForLoginDTO.OSVersion;
                deviceInfo.NotificationToken = deviceForLoginDTO.NotificationToken;
                deviceInfo.DeviceId = deviceForLoginDTO.DeviceId;
                deviceInfo.ModifiedBy = deviceForLoginDTO.UserId;

                await _userDeviceDal.UpdateAsync(deviceInfo);
            }

            return new SuccessDataResult<UserDevice>(deviceInfo);
        }
    }
}
