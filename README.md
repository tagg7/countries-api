# NordCloud Coding Test
## Features
### Implemented
- .NET Web API with endpoints providing the functionality described in the requirements
- CQRS pattern for handling requests
- MongoDb for document storage
- Redis for caching documents
- Integration tests (Note that MongoDb and Redis containers are started when running the tests; should work locally if you have Docker installed)
- CLEAN architecture
### Partially Implemented
- CI/CD pipeline, infrastructure-as-code within Azure (only wrote a basic build.yml file in infrastructure, also needs a deploy.yml and Bicep configuration file for actually deploying)
### Not Implemented
- Unit tests (and more integration tests outside of happy path)
- Middleware for handling errors and returning correct HTTP status codes
- Middleware for validating request object properties
- Not handling any non-happy path cases such as document doesn't exist, invalid properties, etc.
- Paging for endpoint that returns all data
- Current setup of MongoDb and Redis does not guarantee atomicity (eg. Redis instance could return an outdated document)
- Reading all configuration from configuration file