namespace Market.Application.Modules.IGDB.Queries.GetIGDBGameDetails
{
    public sealed class GetIGDBGameDetailsQueryValidator : AbstractValidator<GetIGDBGameDetailsQuery>
    {
        public GetIGDBGameDetailsQueryValidator()
        {
            RuleFor(x => x.GameId)
                 .GreaterThan(0).WithMessage("Game id must be greater than 0!");
     
        }
    }
}
