namespace Market.Application.Modules.Screenshots.Commands.Delete
{
    public sealed class DeleteScreenshotsCommandHandler : IRequestHandler<DeleteScreenshotsCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteScreenshotsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
