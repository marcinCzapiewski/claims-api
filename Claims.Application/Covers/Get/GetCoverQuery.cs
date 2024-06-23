using Claims.Domain.Covers;
using MediatR;

namespace Claims.Application.Covers.Get;
public sealed record GetCoverQuery(string CoverId) : IRequest<Cover?>;
