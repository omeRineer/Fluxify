using FirebaseAdmin;
using Fluxify.Application.Infrastructure;
using Fluxify.Infrastructure.Services;
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
            services.AddScoped<INotificationService, FirebaseNotificationService>();

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile($"{AppDomain.CurrentDomain.BaseDirectory}/External/fluxify-adminsdk.json")
            });
        }
    }
}
