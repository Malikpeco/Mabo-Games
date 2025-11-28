using Market.Application.Modules.RegisterUser.Dto;
using Market.Application.Abstractions;
using Market.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Market.Application.Modules.RegisterUser.Commands.Register;

public sealed class RegisterUserCommandHandler(
    IAppDbContext context
    ) : IRequestHandler<RegisterUserCommand, RegisterUserResultDto>
{
    public async Task<RegisterUserResultDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        bool exists = await context.Users.AnyAsync(x => x.Username == request.Username || x.Email==request.Email, cancellationToken);

        var passwordHasher = new PasswordHasher<UserEntity>();

       
        if (exists)
        {
            throw new MarketConflictException("User with that username/email adress already exists");
        }

        var user = new UserEntity()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Username,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            
            CreatedAtUtc = DateTime.UtcNow,
            IsAdmin = false,
            IsDeleted = false,
            IsEnabled=true,
            CountryId = request.CountryId
        };

        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

        var cart = new CartEntity()
        {
            User = user,
        };

        context.Users.Add(user);
        context.Carts.Add(cart);


        await context.SaveChangesAsync(cancellationToken);

        return new RegisterUserResultDto()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
        };

    }
}
