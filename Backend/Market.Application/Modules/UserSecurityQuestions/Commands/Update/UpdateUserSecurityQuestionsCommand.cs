using Market.Domain.Common.Attributes;

namespace Market.Application.Modules.UserSecurityQuestions.Commands.Update
{
    [PreserveString]
    public sealed class UpdateUserSecurityQuestionsCommand: IRequest<int>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string newAnswer { get; set; }
    }
}
