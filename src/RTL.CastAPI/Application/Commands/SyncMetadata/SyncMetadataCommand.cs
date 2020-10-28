using MediatR;

namespace RTL.CastAPI.Application.Commands.SyncMetadata
{
    public class SyncMetadataCommand : IRequest<SyncMetadataCommandResult>
    {
        public int SyncBatchSize { get; } = 20;
    }
}
