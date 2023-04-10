namespace Status.Infrastructure.Services
{
    public interface IResponseService
    {
        public Task RequestStatusFromAllServers(CancellationToken cancellationToken);
    }
}
