RTL Cast API
=====

The RTL Cast API provides the following functionality:
- Scrape shows/cast metadata from [TvMaze](http://www.tvmaze.com/api)
- Provide this information via a REST API

The REST API also provides a test interface via [Swagger](https://swagger.io/). NB. Please note that HttpsRedirection is enabled.

========

### Implementation Remarks

- Scrapping runs as an HostedService - it kicks off in the background when the application starts. It will trigger scrapping every 12 hours (configurable).
- Scrapped data is persisted in memory fow now. That means all scrapped data will be lost once the application terminates and scrapping will start from zero once the application restarts.
- Project structure could be split into something like [this](https://github.com/ardalis/CleanArchitecture) - but for now KISS.
- Our model is TIGHTLY coupled with the TvMaze model - we're even using external ids as our ids! :O :O :O We should definitely avoid that in a real system. Persisting the mapping from external source to our internal ids [ExternalSource|ExternalShowId|InternalShowId] can resolve this.

========

### Start

Run Unit Tests
```
> cd tests\RTL.CastAPI.UnitTests
> dotnet test
```

Run Application
```
> cd src\RTL.CastAPI
> dotnet run
```

