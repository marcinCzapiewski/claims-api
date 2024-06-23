using Claims.Domain.Shared;
using MediatR;

namespace Claims.Application.Covers.Commands;
public sealed record DeleteCoverCommand : IRequest<Result>
{
    public required string CoverId { get; init; }
    public required string HttpRequestType { get; init; }
}
