using Claims.Domain.Covers;
using MediatR;

namespace Claims.Application.Covers.Create;
public sealed record CreateCoverCommand : IRequest<Cover>
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public CoverType Type { get; init; }
    public required string HttpRequestType { get; init; }
}
