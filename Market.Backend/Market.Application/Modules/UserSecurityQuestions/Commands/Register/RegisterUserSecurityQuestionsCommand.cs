using Market.Domain.Common.Attributes;
using Market.Domain.Entities;

namespace Market.Application.Modules.UserSecurityQuestions.Commands.Create
{
    [PreserveString]
    public sealed class RegisterUserSecurityQuestionsCommand : IRequest<int>
    {
        public int SecurityQuestionId {  get; set; }
        public string SecurityQuestionAnswer { get; set; }


    }
}
