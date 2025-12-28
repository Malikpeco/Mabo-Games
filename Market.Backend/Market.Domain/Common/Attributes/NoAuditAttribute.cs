using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Common.Attributes
{
    /// <summary>
    /// Properties or Classes marked with this attribute will be ignored by the Audit Logging mechanism.
    /// Use for sensitive data like passwords hashes, jwt tokes, etc...
    /// </summary>

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public sealed class NoAuditAttribute:Attribute
    {
    }
}
