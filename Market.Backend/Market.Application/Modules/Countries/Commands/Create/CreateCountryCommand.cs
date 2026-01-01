using Market.Domain.Common.Attributes;

namespace Market.Application.Modules.Countries.Commands.Create
{
    public sealed class CreateCountryCommand : IRequest<Unit>
    {
        [PreserveCapitalization]
        public required string Name { get; set; }
        
    }
}
