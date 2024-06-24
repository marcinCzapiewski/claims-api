namespace Claims.Events;

public record CoverCreatedEvent
{
    public required string CoverId { get; init; }
    public required string HttpRequestType { get; init; }
}
