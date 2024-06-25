namespace Claims.Domain.Covers;
public record CoverReadModel
{
    public required string Id { get; init; }

    public DateTime StartDate { get; init; }

    public DateTime EndDate { get; init; }

    public CoverType Type { get; init; }

    public decimal Premium { get; init; }
}
