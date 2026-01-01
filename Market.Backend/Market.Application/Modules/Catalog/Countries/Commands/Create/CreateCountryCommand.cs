using Market.Domain.Common.Attributes;

namespace Market.Application.Modules.Countries.Commands.Create
{
    public class CreateCountryCommand : IRequest<int>
    {
        [PreserveCapitalization]
        public required string Name { get; set; }
        
    }
}
