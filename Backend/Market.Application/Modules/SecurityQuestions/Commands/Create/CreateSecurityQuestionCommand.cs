using Market.Application.Modules.SecurityQuestions.Dto;
using Market.Domain.Common.Attributes;

namespace Market.Application.Modules.SecurityQuestions.Commands.Create
{

    [PreserveString]
    public sealed class CreateSecurityQuestionCommand : IRequest<CreateSecurityQuestionResultDto>
    {
        public string Question { get; set; }
    }
}
