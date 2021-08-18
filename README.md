# Usermap Api
This is a library that can be used to access Usermap api (https://kosapi.fit.cvut.cz/usermap/doc/rest-api-v1.html, https://rozvoj.fit.cvut.cz/Main/usermap-api).

This repository does not contain all entities for usermap api, only those needed in Christofel project. If you are using entity that is not present here, you can open PR with it.

Library contains caching mechanism (using `Microsoft.Extensions.Caching`) and is supposed to be used as scoped service, but may be used as a singleton if you set up caching correctly.

# How to use

1. instance of `UsermapApi` class must be obtained
2. `UsermapApi.GetAuthorizedApi` will return `AuthorizedUsermapApi` class that is used to access the API with given access token.
3. Use some of the exposed properties that represent controllers and methods inside them that load entities.

Example with dependency injection can be found in `Usermap.Example` project
