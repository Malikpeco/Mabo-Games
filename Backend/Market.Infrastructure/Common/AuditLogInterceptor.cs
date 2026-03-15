using Market.Application.Abstractions;
using Market.Domain.Common.Attributes;
using Market.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Market.Infrastructure.Common
{
    public sealed class AuditLogInterceptor:SaveChangesInterceptor
    {
        private readonly IAppCurrentUser appCurrentUser;
        

        public AuditLogInterceptor(IAppCurrentUser appCurrentUser)
        {
            this.appCurrentUser = appCurrentUser;
        }


        public override ValueTask<InterceptionResult<int>> SavingChangesAsync
            (DbContextEventData eventData,
            InterceptionResult<int> interceptionResult,
            CancellationToken cancellationToken)
        {

            var context = eventData.Context;
            if(context is null) 
                return base.SavingChangesAsync(eventData, interceptionResult, cancellationToken);

            var auditLogs = context.ChangeTracker.Entries()
                .Where(x=>x.Entity is not AuditLogEntity && // a little redundant checking if is not AuditLogEntity but for security its better
                    !Attribute.IsDefined(x.Entity.GetType(), typeof(NoAuditAttribute))&&
                    (x.State == EntityState.Added ||
                     x.State == EntityState.Modified ||
                     x.State == EntityState.Deleted))
                .Select(createAuditLog)
                .ToList();



            if(auditLogs.Any())
                context.Set<AuditLogEntity>().AddRange(auditLogs);


            return base.SavingChangesAsync(eventData, interceptionResult, cancellationToken);

        }


        private AuditLogEntity createAuditLog(EntityEntry entry)
        {

            return new AuditLogEntity()
            {
                UserId = appCurrentUser.IsAuthenticated?
                    appCurrentUser.UserId : null,

                EntityName = entry.Entity.GetType().Name,

                EntityId = entry.State == EntityState.Added ?
                null : entry.Property("Id").CurrentValue!.ToString(),

                BeforeChange = entry.State == EntityState.Added ?
                null : JsonSerializer.Serialize(Sanitize(entry, entry.OriginalValues)),


                AfterChange = entry.State == EntityState.Deleted ?
                null : JsonSerializer.Serialize(Sanitize(entry, entry.CurrentValues)),

                Action = entry.State switch
                {
                    EntityState.Added => "Create",
                    EntityState.Modified => "Update",
                    EntityState.Deleted => "Delete",
                    _ => "Unknown"
                },

                CreatedAtUtc= DateTime.UtcNow,

                IsDeleted=false,

            };


        }

        private object Sanitize(EntityEntry entry,PropertyValues propertyValues)
        {
            var entityType = entry.Entity.GetType();

            return propertyValues.Properties
                .Where(x =>
                {
                    var propName = entityType.GetProperty(x.Name);

                    if(propName is null)
                        return true; // Include shadow properties (no way to mark them NoAudit)

                    return !Attribute.IsDefined(propName, typeof(NoAuditAttribute));
                }).
                ToDictionary(
                    x => x.Name,
                    x => propertyValues[x.Name]
                );


        }


    }
}
