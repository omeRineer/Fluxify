using Fluxify.Application.Repositories;
using Fluxify.Application.Repositories.Base;
using Fluxify.Domain.Core;
using MongoDB.Driver;

namespace Fluxify.Persistance.Repositories
{
    public class UserDeviceRepository : MongoRepository<UserDevice, Guid>, IUserDeviceRepository
    {
        public UserDeviceRepository(IMongoDatabase dataBase) : base(dataBase, "UserDevices")
        {
        }
    }
}
