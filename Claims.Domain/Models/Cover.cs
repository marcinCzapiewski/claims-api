using Claims.Domain.Models;

namespace Claims.Domain.Entities;

public class Cover
{
    public string Id { get; }

    public DateTime StartDate { get; }

    public DateTime EndDate { get; }

    public CoverType Type { get; }

    public decimal Premium { get; }

    public Cover(string id, DateTime startDate, DateTime endDate, CoverType type, decimal premium)
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        Type = type;
        Premium = premium;
    }

}