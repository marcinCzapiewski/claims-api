using Claims.Application.Covers;

namespace Claims.Api.Covers;

internal static class Mapper
{
    public static Contracts.Cover ToCoverApiContract(this CoverDto cover) => new()
    {
        Id = cover.Id,
        StartDate = cover.StartDate,
        EndDate = cover.EndDate,
        Premium = cover.Premium,
        Type = (Contracts.CoverType)cover.Type
    };

    public static IEnumerable<Contracts.Cover> ToCoverApiContracts(this IEnumerable<CoverDto> covers) =>
        covers.Select(x => x.ToCoverApiContract());

    public static Contracts.Cover ToCoverApiContract(this Domain.Covers.Cover cover) => new()
    {
        Id = cover.Id,
        StartDate = cover.StartDate,
        EndDate = cover.EndDate,
        Premium = cover.Premium,
        Type = (Contracts.CoverType)cover.Type
    };
}
