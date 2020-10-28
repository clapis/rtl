namespace RTL.CastAPI.Application.Commands.ScrapeShows
{
    public class ScrapeShowsCommandResult
    {
        public bool IsSuccess { get; private set; }

        private ScrapeShowsCommandResult(bool success) 
        { 
            IsSuccess = success; 
        }

        public static ScrapeShowsCommandResult Success() 
            => new ScrapeShowsCommandResult(true);
    }
}