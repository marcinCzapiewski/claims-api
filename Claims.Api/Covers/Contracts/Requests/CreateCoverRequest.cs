namespace Claims.Api.Contracts.Requests;

public record CreateCoverRequest
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public CoverType Type { get; init; }
}
