namespace Market.Application.Modules.Users.Commands.UploadProfileImage;

public sealed class UploadProfileImageCommandValidator : AbstractValidator<UploadProfileImageCommand>
{
    private const long MaxFileSize = 10 * 1024 * 1024;

    public UploadProfileImageCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull()
            .WithMessage("File is required");

        When(x => x.File is not null, () =>
        {
            RuleFor(x => x.File.Length)
                .GreaterThan(0)
                .WithMessage("File cannot be empty")
                .LessThanOrEqualTo(MaxFileSize)
                .WithMessage("File size must not exceed 10MB");

            RuleFor(x => x.File.ContentType)
                .Must(x => x.StartsWith("image/"))
                .WithMessage("File must be an image");
        });
    }
}