using Entities.Concrete;
using Entities.DTOs.Params;
using Core.Entities.DTOs;

namespace Business.Abstract
{
    public interface IUserDeviceService
    {
        Task<IDataResult<UserDevice>> UpsertAsync(DeviceForLoginDTO deviceForLoginDTO, string ipAddress, string browserName);
    }
}
