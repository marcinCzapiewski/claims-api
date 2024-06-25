namespace Claims.Domain.Claims;
public record ClaimReadModel
{
    public required string Id { get; init; }
    public required string CoverId { get; init; }
    public DateTime Created { get; init; }
    public required string Name { get; init; }
    public ClaimType Type { get; init; }
    public decimal DamageCost { get; init; }
}
