namespace Claims.Events;
public record ClaimDeletedEvent
{
    public required string ClaimId { get; init; }
    public required string HttpRequestType { get; init; }
}
