using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Core;

namespace Fluxify.Application.Repositories
{
    public interface IUserDeviceRepository : IRepository<UserDevice, Guid>
    {
    }
}
