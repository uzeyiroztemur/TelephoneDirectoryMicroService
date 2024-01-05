using Entities.Concrete;
using Entities.DTOs.Params;
using Core.Entities.DTOs;

namespace Business.Abstract
{
    public interface IUserDeviceService
    {
        IDataResult<UserDevice> Upsert(DeviceForLoginDTO deviceForLoginDTO, string ipAddress, string browserName);
    }
}
