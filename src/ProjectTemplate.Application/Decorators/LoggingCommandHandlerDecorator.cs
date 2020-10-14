using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ProjectTemplate.Application.Abstractions.Commands;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace ProjectTemplate.Application.Decorators
{
    public class LoggingCommandHandlerDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>

    {
        private readonly ILogger _logger;

        public LoggingCommandHandlerDecorator(
            ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {

            try
            {
                _logger.Information(
                    "Executing command/query {Command}",
                    typeof(TRequest).GetType().Name);

                var result = await next();

                _logger.Information("Command/Query {Command} processed successful", typeof(TRequest).GetType().Name);

                return result;
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Command/Query {Command} processing failed", typeof(TRequest).GetType().Name);
                throw;
            }

        }
    }
}