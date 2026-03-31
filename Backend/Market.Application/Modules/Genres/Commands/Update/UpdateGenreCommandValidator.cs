namespace Market.Application.Modules.Genres.Commands.Update
{
    public sealed class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
    {
        public UpdateGenreCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Genre name must not be empty!")
                .MinimumLength(2);
        }
    }
}
