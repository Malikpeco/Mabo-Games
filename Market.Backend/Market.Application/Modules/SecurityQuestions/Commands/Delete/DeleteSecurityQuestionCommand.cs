using Market.Application.Modules.SecurityQuestions.Dto;

namespace Market.Application.Modules.SecurityQuestions.Commands.Delete
{
    public sealed class DeleteSecurityQuestionCommand : IRequest<DeleteSecurityQuestionResultDto>
    {
        public int Id { get; set; }
    }
}
