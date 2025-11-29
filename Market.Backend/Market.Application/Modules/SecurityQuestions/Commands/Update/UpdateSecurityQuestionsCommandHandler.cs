namespace Market.Application.Modules.SecurityQuestions.Commands.Update
{
    public sealed class UpdateSecurityQuestionsCommandHandler : IRequestHandler<UpdateSecurityQuestionsCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateSecurityQuestionsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
