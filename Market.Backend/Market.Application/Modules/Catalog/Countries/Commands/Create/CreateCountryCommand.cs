namespace Market.Application.Modules.Countries.Commands.Create
{
    public class CreateCountryCommand : IRequest<int>
    {
        public required string Name { get; set; }
        
    }
}
