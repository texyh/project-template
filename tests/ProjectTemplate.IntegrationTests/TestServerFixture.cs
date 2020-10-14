using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ProjectTemplate.Api;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Environment = ProjectTemplate.Api.Environment;

namespace ProjectTemplate.IntegrationTests
{
    public class TestServerFixture
    {
        private TestServer _testServer;

        public void CreateTestEnvironment(Action<IServiceCollection> addTestCases)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            Environment.SetToDevelopment();
            var settings = new List<KeyValuePair<string, string>>();
            settings.Add(new KeyValuePair<string, string>("BANK_API_URL", "http://localhost:5002"));
            settings.AddRange(
                new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("POSTGRES_USERNAME", "root"),
                    new KeyValuePair<string, string>("POSTGRES_PASSWORD", "Pass@word1"),
                    new KeyValuePair<string, string>("POSTGRES_HOST", "localhost"),
                    //new KeyValuePair<string, string>("POSTGRES_HOST", "192.168.99.100"),
                    new KeyValuePair<string, string>("POSTGRES_DB_NAME", "paymanetGatewayDb"),
                    new KeyValuePair<string, string>("POSTGRES_PORT", "5432"),
                }
            );
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
            var server = new TestServer(new WebHostBuilder()
                .UseConfiguration(configuration)
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    addTestCases(services);
                    services.AddMartenDB(configuration);
                }).UseSerilog(Log.Logger));

            _testServer =  server;
        }

        public T GetService<T>()
        {
            CheckNotNull(_testServer);

            return _testServer.Services.GetService<T>();
        }

        public async Task<TResponse> PostAsync<TResponse, TRequest>(string uri, TRequest request, CancellationToken cancellationToken)
        {
            CheckNotNull(_testServer);

            var httpClient = _testServer.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var httpResponse = await httpClient.PostAsync(uri, content, cancellationToken);
            
            return JsonConvert.DeserializeObject<TResponse>(await httpResponse.Content.ReadAsStringAsync());
        }

        public async Task<TResponse> GetAsync<TResponse>(string uri, CancellationToken cancellationToken)
        {
            CheckNotNull(_testServer);

            var httpClient = _testServer.CreateClient();
            var httpResponse = await httpClient.GetAsync(uri, cancellationToken);

            return JsonConvert.DeserializeObject<TResponse>(await httpResponse.Content.ReadAsStringAsync());
        }

        public void CheckNotNull(TestServer server)
        {
            if(server == null)
            {
                throw new OperationException("the test server is null, please call \"CreateTestEnvironment\" before using this Fixture");
            }
        }
    }
}
