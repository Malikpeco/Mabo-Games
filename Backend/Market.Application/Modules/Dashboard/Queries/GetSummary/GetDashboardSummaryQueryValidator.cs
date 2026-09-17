namespace Market.Application.Modules.Dashboard.Queries.GetSummary
{
    public sealed class GetDashboardSummaryQueryValidator : AbstractValidator<GetDashboardSummaryQuery>
    {
        private const int MaxRangeDays = 730; // 2 years

        public GetDashboardSummaryQueryValidator()
        {
            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate!.Value)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                .WithMessage("ToDate must be on or after FromDate.");

            RuleFor(x => x)
                .Must(x => !x.FromDate.HasValue || !x.ToDate.HasValue || (x.ToDate!.Value - x.FromDate!.Value).TotalDays <= MaxRangeDays)
                .WithMessage($"Date range cannot exceed {MaxRangeDays} days.");
        }
    }
}
