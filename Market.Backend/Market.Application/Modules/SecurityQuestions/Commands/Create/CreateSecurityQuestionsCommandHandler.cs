namespace Market.Application.Modules.SecurityQuestions.Commands.Create
{
    public sealed class CreateSecurityQuestionsCommandHandler : IRequestHandler<CreateSecurityQuestionsCommand, int>
    {
        public async Task<int> Handle(CreateSecurityQuestionsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
