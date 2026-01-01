using Market.Application.Modules.SecurityQuestions.Dto;
using Market.Domain.Common.Attributes;

namespace Market.Application.Modules.SecurityQuestions.Commands.Update
{

    [PreserveString]
    public sealed class UpdateSecurityQuestionCommand : IRequest<UpdateSecurityQuestionResultDto>
    {
        [JsonIgnore]
        public int Id {  get; set; }

        public string NewQuestion {  get; set; }

    }
}
