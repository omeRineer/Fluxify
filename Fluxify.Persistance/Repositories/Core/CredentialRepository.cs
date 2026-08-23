using Fluxify.Application.Repositories;
using Fluxify.Domain.Entities.Core;
using Fluxify.Persistance.Context;
using Fluxify.Persistance.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Persistance.Repositories.Core
{
    public class CredentialRepository : CoreRepository<Credential, Guid>, ICredentialRepository
    {
        public CredentialRepository(CoreContext context) : base(context)
        {
        }
    }
}
