using MongoDB.Bson.Serialization.Attributes;

namespace Claims.Api.Contracts;

public class Cover
{
    public Guid Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public CoverType Type { get; set; }

    public decimal Premium { get; set; }
}
