using Market.Application.Modules.SecurityQuestions.Dto;

namespace Market.Application.Modules.SecurityQuestions.Commands.Create
{
    public sealed class CreateSecurityQuestionCommand : IRequest<CreateSecurityQuestionResultDto>
    {
        public string Question { get; set; }
    }
}
