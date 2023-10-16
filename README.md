# WebAPI
Continuous Integration / Continuous Development Document

I have an API service that I just created an update for. I have made the Pull Request to merge into master.

We will now begin the first step of the CI/CD pipeline:

During the Pull Request step, I have a Github Actions (GHA) process that:

- Builds the service
- Ensures that the services are built successfully and pass these tests
- Ensure all eng standards defined by the team have been met

If these are successful, we can merge this code and go to the CI step:

CI step:

- The CI step could also be an Github Action. (GHA). When the code in merged into master, this step is triggered.
In this step, we can build the service again and release a new version to Artifactory, a place where we store new versions of a library/service
Once this is successful, we’ll go to the CD stage

CD step:

We can use Spinnaker for the CD step. 
This step will only be triggered if the CI step is successful (a new version has been released to Artifactory, the build is successful and so on)
We release the new build of this service to the test environment. Let's say we are running a docker service. We’ll deploy the new test instance to Azure and make sure the containers are built and deployed. If this service is not using docker, then we can check out App Services and monitor the status of the service (is the build successful? CPU usage and memory usage normal?)
After the test step is successful, we can move on to deploy into prod-canary (deploy to production and receive only 5% of traffic)
We’ll monitor this service on Azure again. Is everything normal? Are all the metrics good?
During this CD process, we’ll run E2E tests and Performance tests on production to ensure it is responding properly in that environment

Possible gaps:

Logging: 
Gap: There is no comprehensive logging mechanism for tracking API usage, errors, or security events.
Improvement: Implement logging using a  logging framework (such as Serilog or NLog) to record API activities. Integrate with monitoring tools to receive alerts for critical issues.

Security:
Gap: The current service does not include any authentication or authorization mechanisms.
Improvement: Implement authentication (such as JWT tokens) and authorization (e.g., role-based access control) to secure the API endpoints. Validate and sanitize input to prevent potential security vulnerabilities.

Performance Optimization:
Gap: If the application has lots of traffic, performance considerations for cryptographic operations and URL validation should be addressed. 
Improvement: Optimize cryptographic operations for speed and security. Consider caching validated URLs for a certain period of time to reduce redundant validation requests.
Could also introduce threading or deploy API in a load-balancing environment. Could also implement rate-limit. 
