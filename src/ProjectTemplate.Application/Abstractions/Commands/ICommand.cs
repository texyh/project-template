using System;
using MediatR;

namespace ProjectTemplate.Application.Abstractions.Commands
{
    public interface ICommand : IRequest
    {
        
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
        
    }
}