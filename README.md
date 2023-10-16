# WebAPI
Continuous Integration / Continuous Development Document

I have an API service that I just created an update for. I have made the Pull Request to merge into master.

We will now begin the first step of the CI/CD pipeline:

During the Pull Request step, I have a Github Actions (GHA) process that:

- Builds the service
- Ensures that the services builds successfully and passes these tests
- Ensure all eng standards defined by the team have been met

If these are successful, we can merge this code and go to the CI step:

CI step:

- The CI step could also be an Github Action. (GHA). When the code in merged into master, this step is triggered.
In this step, we can build the service again and release a new version to Artifactory, a place where we store new versions of a library/service
Once this is successful, we’ll go to the CD stage

CD step:

We can use Spinnaker for the CD step. 
This step will only be triggered if the CI step is successful (a new version has been released to Artifactory, build is successful and so on)
We release the new build of this service to the test environment. Lets say we are running a docker service. We’ll deploy the new test instance to Azure and make sure the containers are built and deployed. If this service is not using docker, then we can check out App Services and monitor the status of the service (is the build successful? CPU usage and memory usage normal?)
After the test step is successful, we can move on to deploy into prod-canary (deploy to production and receive only 5% of traffic)
We’ll monitor this service on Azure again. Is everything normal? Are all the metrics good?
During this CD process, we’ll run E2E tests and Performance tests on production to ensure it is respondingly properly in that enviornment
