using Claims.Domain.Covers;
using MediatR;

namespace Claims.Application.Covers.Queries;
public sealed record GetCoversQuery : IRequest<IReadOnlyCollection<CoverReadModel>>;
