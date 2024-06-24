namespace Claims.Events;
public record ClaimCreatedEvent
{
    public required string ClaimId { get; init; }
    public required string HttpRequestType { get; init; }
}
