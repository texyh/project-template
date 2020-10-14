using System;
using System.Net.Http;
using Polly;
using Polly.Extensions.Http;

namespace ProjectTemplate.Infrastructure.AcquiringBank
{
    public static class RetryPolicy
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryTimes)
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(retryTimes, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                    retryAttempt)));
        }
    }
}