namespace Market.Application.Modules.Auth.Commands.UserSecurityQuestions.Commands.Create
{
    public sealed class CreateUserSecurityQuestionsCommandHandler : IRequestHandler<CreateUserSecurityQuestionsCommand, int>
    {
        public async Task<int> Handle(CreateUserSecurityQuestionsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
