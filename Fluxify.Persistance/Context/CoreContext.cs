using Fluxify.Domain;
using Fluxify.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Persistance.Context
{
    public class CoreContext : DbContext
    {
        public CoreContext(DbContextOptions<CoreContext> opt) : base(opt)
        {
            
        }

        public DbSet<NameSpace> NameSpaces { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDevice> UserDevices { get; set; }
        public DbSet<UserTopic> UserTopics { get; set; }
        public DbSet<Credential> Credentials { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcessEntries<Guid>();

            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProcessEntries<TKey>()
        {
            var dataEntries = ChangeTracker.Entries<CoreEntity<Guid>>();
            foreach (var data in dataEntries)
            {
                switch (data.State)
                {
                    case EntityState.Detached: break;
                    case EntityState.Unchanged: break;
                    case EntityState.Deleted: break;
                    case EntityState.Modified:
                        data.Entity.EditDate = DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        data.Entity.CreateDate = DateTime.UtcNow;
                        data.Entity.EditDate = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
