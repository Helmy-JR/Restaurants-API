using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands
{
    public class UpdateUserDetailsCommandHandler : IRequestHandler<UpdateUserDetailsCommand>
    {
        private readonly ILogger _logger;
        private readonly IUserContext _userContext;
        private readonly IUserStore<User> _userStore;

        public UpdateUserDetailsCommandHandler
        (
            ILogger<UpdateUserDetailsCommand> logger,
            IUserContext userContext,
            IUserStore<User> userStore
        )
        {
            _logger = logger;
            _userContext = userContext;
            _userStore = userStore;
        }

        public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();

            _logger.LogInformation("Updating User: {UserId}, with {@Request}", user!.Id ,request);

            var dbUser = await _userStore.FindByIdAsync(user!.Id,cancellationToken);
            
            if(dbUser is null)
            {
                throw new NotFoundException(nameof(User), user!.Id);
            }
            
            dbUser.Nationality = request.Nationality;
            dbUser.DateOfBirth = request.DateFOfBirth;

            await _userStore.UpdateAsync(dbUser, cancellationToken);
        }
    }
}