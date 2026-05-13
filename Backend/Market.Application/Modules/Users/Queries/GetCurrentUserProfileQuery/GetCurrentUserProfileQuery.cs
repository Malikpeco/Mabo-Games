using Market.Application.Modules.Users.Dto;

namespace Market.Application.Modules.Users.Queries.GetCurrentUserProfileQuery
{
    public sealed record GetCurrentUserProfileQuery : IRequest<GetUserProfileQueryDto>
    {
    }
}
