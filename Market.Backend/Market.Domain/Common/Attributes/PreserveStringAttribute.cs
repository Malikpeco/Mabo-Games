using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Common.Attributes
{
    /// <summary>
    /// Properties or entire classes marked with this attribute will be skipped during string normalization processes.
    /// Use for properties like passwords, codes, identifiers, etc...
    /// </summary>

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public sealed class PreserveStringAttribute : Attribute
    {
    }
}
