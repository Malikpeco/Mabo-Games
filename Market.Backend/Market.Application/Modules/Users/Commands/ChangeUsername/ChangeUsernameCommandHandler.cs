using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Modules.Users.Commands.ChangeUsername
{
    public sealed class ChangeUsernameCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser,
        IEmailSender emailSender, IPasswordHasher<UserEntity> passwordHasher) : IRequestHandler<ChangeUsernameCommand, Unit>
    {
        public async Task<Unit> Handle(ChangeUsernameCommand request, CancellationToken cancellationToken)
        {
            if (!appCurrentUser.IsAuthenticated)
                throw new MarketForbiddenException();

            var user = await context.Users.FirstAsync(x=>x.Id==appCurrentUser.UserId,cancellationToken);


            var passwordsMatch = passwordHasher.VerifyHashedPassword(user,user.PasswordHash,request.Password);

            if (passwordsMatch == PasswordVerificationResult.Failed)
                throw new MarketForbiddenException(403,"Entered password is incorrect, try again.");

            string oldUsername=user.Username;

            var usernameExists = await context.Users.AnyAsync(x=>x.Username==request.NewUsername,cancellationToken);

            if (oldUsername == request.NewUsername)
                throw new MarketConflictException("Username cannot be the same as your old one.");

            if (usernameExists)
                throw new MarketConflictException("Username already exists, try again.");
           

            
            user.Username = request.NewUsername;

            await context.SaveChangesAsync(cancellationToken);


            await emailSender.SendEmail(user.Email, "Username Change",
                $"Hi {user.FirstName},\nYour username has been successfully changed from {oldUsername} to {request.NewUsername}" +
                $"\n\nIf you did not request a username change, please contact our support team.",cancellationToken);


            return Unit.Value;
        }
    }
}
