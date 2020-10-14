using MediatR;

namespace ProjectTemplate.Application.Abstractions.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {

    }
}