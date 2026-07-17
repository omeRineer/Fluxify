using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Repositories.Base;
using Fluxify.Application.Utilities.Result;
using Fluxify.Domain.Entities.Core;

namespace Fluxify.Application.Repositories
{
    public interface IUserRepository : IEfCoreRepository<User, Guid>
    {
    }
}
