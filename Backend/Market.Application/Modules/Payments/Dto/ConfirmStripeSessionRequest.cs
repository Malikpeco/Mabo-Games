namespace Market.Application.Modules.Payments.Dto
{
    public sealed class ConfirmStripeSessionRequest
    {
        public string SessionId { get; set; } = string.Empty;
    }
}
