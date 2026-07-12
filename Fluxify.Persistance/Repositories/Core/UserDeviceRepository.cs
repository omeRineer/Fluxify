using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Entities.Core;
using Fluxify.Persistance.Context;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories.Core
{
    public class UserDeviceRepository : CoreRepository<UserDevice, Guid, CoreContext>, IUserDeviceRepository
    {
        public UserDeviceRepository(CoreContext dbContext) : base(dbContext)
        {
        }
    }
}
