namespace Market.Application.Modules.SecurityQuestions.Commands.Delete
{
    public sealed class DeleteSecurityQuestionsCommandHandler : IRequestHandler<DeleteSecurityQuestionsCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteSecurityQuestionsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
