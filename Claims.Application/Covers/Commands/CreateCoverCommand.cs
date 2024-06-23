using Claims.Domain.Covers;
using Claims.Domain.Shared;
using MediatR;

namespace Claims.Application.Covers.Commands;
public sealed record CreateCoverCommand : IRequest<Result<Cover>>
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public CoverType Type { get; init; }
    public required string HttpRequestType { get; init; }
}
