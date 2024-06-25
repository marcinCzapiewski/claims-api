using Claims.Domain.Covers;

namespace Claims.Application.Covers;
public static class Mapper
{
    public static CoverReadModel ToReadModel(this Cover cover) => new()
    {
        Id = cover.Id,
        StartDate = cover.StartDate,
        EndDate = cover.EndDate,
        Premium = cover.Premium.Value,
        Type = cover.Type
    };
}
