namespace Market.Application.Abstractions
{
    public interface IRecaptchaService
    {
        Task<bool> VerifyAsync(string token, CancellationToken ct);
    }
}
