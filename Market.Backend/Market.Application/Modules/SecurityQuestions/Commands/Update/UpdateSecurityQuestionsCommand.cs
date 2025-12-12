using Market.Application.Modules.SecurityQuestions.Dto;

namespace Market.Application.Modules.SecurityQuestions.Commands.Update
{
    public sealed class UpdateSecurityQuestionsCommand : IRequest<UpdateSecurityQuestionResultDto>
    {
        [JsonIgnore]
        public int Id {  get; set; }

        public string NewQuestion {  get; set; }

    }
}
