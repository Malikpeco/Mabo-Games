namespace Market.Application.Modules.Countries.Commands.Create
{
    public sealed class CreateCountryCommand : IRequest<Unit>
    {
        public required string Name { get; set; }
        
    }
}
