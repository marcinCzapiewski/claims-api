namespace Claims.Application.Commands;
public record DeleteCoverCommand
{
    public required string CoverId { get; init; }
    public required string HttpRequestType { get; init; }
}
