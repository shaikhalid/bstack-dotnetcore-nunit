# bstack-dotnetcore-nunit

# Add BrowserStack Credentials
- Update your BrowserStack username and access key details in the "appSettings.json" (https://github.com/nithyamn/bstack-dotnetcore-nunit/blob/main/BstackNetCoreNunit/appSettings.json) file
- Get your credentials from here https://www.browserstack.com/accounts/settings

# Commands to trigger tests
- Build project - `dotnet build`
- Single test - `dotnet test --filter SingleTestCase`. `SingleTestCase` is the name of the test method under `[Test]` annotation to be executed.
- Local test - `dotnet test --filter LocalTestCase` 
- Parallel test - `dotnet test --filter ParallelTestCase -- NUnit.NumberOfTestWorkers=4`. `NUnit.NumberOfTestWorkers` is basically setting the no.of parallel threads to be triggered concurrently.