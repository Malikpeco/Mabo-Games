using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Common.Attributes
{

    /// <summary>
    /// Properties marked with this attribute will have their capitalization preserved during string normalization processes.
    /// Use for properties like usernames, titles or bios
    /// Properties will still be trimmed but capitalization will remain unchanged.
    /// </summary>

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PreserveCapitalizationAttribute: Attribute
    {
    }
}
