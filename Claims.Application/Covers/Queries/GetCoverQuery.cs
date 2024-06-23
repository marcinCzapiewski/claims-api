using Claims.Domain.Covers;
using MediatR;

namespace Claims.Application.Covers.Queries;
public sealed record GetCoverQuery(string CoverId) : IRequest<CoverDto?>;
