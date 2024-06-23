namespace Claims.Api.Covers;

internal static class Mapper
{
    public static Contracts.Cover ToCoverApiContract(this Domain.Covers.Cover cover) => new()
    {
        Id = cover.Id,
        StartDate = cover.StartDate,
        EndDate = cover.EndDate,
        Premium = cover.Premium,
        Type = (Contracts.CoverType)cover.Type
    };

    public static IEnumerable<Contracts.Cover> ToCoverApiContracts(this IEnumerable<Domain.Covers.Cover> covers) =>
        covers.Select(x => x.ToCoverApiContract());

}
