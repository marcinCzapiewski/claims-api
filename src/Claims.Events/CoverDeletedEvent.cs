namespace Claims.Events;
public class CoverDeletedEvent
{
    public required string CoverId { get; init; }
    public required string HttpRequestType { get; init; }
}
