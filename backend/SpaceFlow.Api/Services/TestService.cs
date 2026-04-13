namespace SpaceFlow.Api.Services;

public class TestService : ITestService
{
    public Task<string> GetStatusAsync()
    {
        return Task.FromResult("SpaceFlow API is running");
    }
}
