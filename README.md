# Usermap Api
This is a library that can be used to access Usermap api (https://kosapi.fit.cvut.cz/usermap/doc/rest-api-v1.html, https://rozvoj.fit.cvut.cz/Main/usermap-api).

Every documented endpoint from v1 is supported with all of the parameters.

# How to obtain
This package is available on NuGet [here](https://www.nuget.org/packages/UsermapApi/).

# How to use
Example with dependency injection and caching can be found in `Basic` project.
For usage with scoped services
(ie. if you are authenticating more users and getting information about them)
look into `Scoped` project.
