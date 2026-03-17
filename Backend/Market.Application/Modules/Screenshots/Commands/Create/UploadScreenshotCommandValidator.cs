namespace Market.Application.Modules.Screenshots.Commands.Create
{
    public sealed class UploadScreenshotValidator : AbstractValidator<UploadScreenshotCommand>
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png"};
        private const long _maxFileSize = 10 * 1024 * 1024; // 10MB

        public UploadScreenshotValidator()
        {

            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required");

            RuleFor(x => x.File.Length)
                .GreaterThan(0)
                .WithMessage("File cannot be empty")
                .LessThanOrEqualTo(_maxFileSize)
                .WithMessage("File size must not exceed 10MB");

            RuleFor(x => x.File.ContentType)
                .Must(x => x.StartsWith("image/"))
                .WithMessage("File must be an image");
                

        }
    }
}
