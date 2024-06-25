using Claims.Domain.Covers;

namespace Claims.Application.Covers;
internal static class CoversMapper
{
    public static Cover ToDomainModel(this CoverDto coverDto) => Cover.LoadFromDatabase(
            coverDto.Id,
            coverDto.StartDate,
            coverDto.EndDate,
            (Domain.Covers.CoverType)coverDto.Type);

    public static CoverDto ToDatabaseModel(this Cover cover) => new()
    {
        Id = cover.Id.ToString(),
        StartDate = cover.StartDate,
        EndDate = cover.EndDate,
        Premium = cover.Premium.Value,
        Type = (CoverType)cover.Type
    };
}
