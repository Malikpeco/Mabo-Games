using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Supabase.Commands
{
    public sealed class UploadGameFileCommandValidator:AbstractValidator<UploadGameFileCommand>
    {

        private const long _maxFileSize = 5 * 1024 * 1024; // 5MB


        public UploadGameFileCommandValidator()
        {

            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File is required");

            RuleFor(x => x.File.Length)
                .GreaterThan(0)
                .WithMessage("File cannot be empty")
                .LessThanOrEqualTo(_maxFileSize)
                .WithMessage("File size must not exceed 5MB");
        }
    }
}
