namespace Market.Application.Modules.Countries.Commands.Create
{
    public class CreateCountryCommandValidator : AbstractValidator<CreateCountryCommand>
    {
        public CreateCountryCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
