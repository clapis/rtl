RTL Cast API
=====

The RTL Cast API provides the following functionality:
- Scrape shows/cast metadata from [TvMaze](http://www.tvmaze.com/api)
- Provide this information via a REST API

The REST API also provides a test interface via [Swagger](https://swagger.io/).

========

### Implementation Remarks

- Scrapping runs as an HostedService - it kicks off in the background when the application starts. It will trigger scrapping every 12 hours (configurable).
- Scrapped data is persisted in memory fow now. That means all scrapped data will be lost once the application terminates and scrapping will start from zero once the application restarts.

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

