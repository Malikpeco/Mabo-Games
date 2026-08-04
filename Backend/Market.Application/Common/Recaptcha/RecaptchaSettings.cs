namespace Market.Application.Common.Recaptcha
{
    public sealed class RecaptchaSettings
    {
        public string SecretKey { get; init; } = default!;
        public string VerifyUrl { get; init; } = "https://www.google.com/recaptcha/api/siteverify";
    }
}
