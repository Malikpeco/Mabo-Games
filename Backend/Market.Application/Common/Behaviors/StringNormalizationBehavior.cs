using Market.Domain.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Common.Behaviors
{
    public sealed class StringNormalizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            StringNormalization(request);

            return await next();

        }

        private void StringNormalization(object obj)
        {
            if (obj == null)
                return;

            var entityType = obj.GetType();

            if (Attribute.IsDefined(entityType, typeof(PreserveStringAttribute)))
                return;

            var entityStringProperties = entityType.GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

            foreach (var property in entityStringProperties)
            {
                var originalValue = property.GetValue(obj) as string;

                if (Attribute.IsDefined(property, typeof(PreserveStringAttribute)))
                    continue;

                string? normalizedValue;

                if (string.IsNullOrWhiteSpace(originalValue))
                    normalizedValue = null;
                else
                    normalizedValue = originalValue.Trim();

                if (normalizedValue != originalValue)
                    property.SetValue(obj, normalizedValue);


            }

        }
    }
}
