using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = CredentialFactory.FromFile<GoogleCredential>("Config/fluxify-adminsdk.json")
            });
        }
    }
}
