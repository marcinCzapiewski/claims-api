using Claims.Domain.Entities;

namespace Claims.Application.Commands;
public record CreateCoverCommand
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public CoverType Type { get; init; }
    public required string HttpRequestType { get; init; }
}
