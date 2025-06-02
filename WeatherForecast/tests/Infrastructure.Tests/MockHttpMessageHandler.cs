namespace Infrastructure.Tests;

public class MockHttpMessageHandler : HttpMessageHandler
{
    public virtual Task<HttpResponseMessage> PublicSendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return SendAsync(request, cancellationToken);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return PublicSendAsync(request, cancellationToken);
    }
}