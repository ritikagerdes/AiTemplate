using Polly;
using Polly.Extensions.Http;

namespace Api.Policies;

public static class HttpPolicyFactory
{
	public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
		HttpPolicyExtensions.HandleTransientHttpError()
			.WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

	public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
		HttpPolicyExtensions.HandleTransientHttpError()
			.CircuitBreakerAsync(3, TimeSpan.FromSeconds(15));

	public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy() =>
		Policy.TimeoutAsync<HttpResponseMessage>(10);
}