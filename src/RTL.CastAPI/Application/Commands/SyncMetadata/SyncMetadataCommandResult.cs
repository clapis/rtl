namespace RTL.CastAPI.Application.Commands.SyncMetadata
{
    public class SyncMetadataCommandResult
    {
        public bool IsSuccess { get; private set; }

        private SyncMetadataCommandResult(bool success)
        {
            IsSuccess = success;
        }

        public static SyncMetadataCommandResult Success()
            => new SyncMetadataCommandResult(true);
    }
}