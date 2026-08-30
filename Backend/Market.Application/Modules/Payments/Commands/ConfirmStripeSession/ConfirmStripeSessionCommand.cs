namespace Market.Application.Modules.Payments.Commands.ConfirmStripeSession
{
    public sealed record ConfirmStripeSessionCommand(string SessionId) : IRequest<ConfirmStripeSessionResponse>;

    public sealed record ConfirmStripeSessionResponse(bool IsSuccess, int? OrderId, string Message);
}
