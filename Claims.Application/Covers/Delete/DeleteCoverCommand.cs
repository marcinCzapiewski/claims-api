using MediatR;

namespace Claims.Application.Covers.Delete;
public sealed record DeleteCoverCommand : IRequest
{
    public required string CoverId { get; init; }
    public required string HttpRequestType { get; init; }
}
