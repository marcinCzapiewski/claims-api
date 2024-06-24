using Claims.Events;
using MassTransit;

namespace Auditing.Application.Consumers;
internal sealed class CoverCreatedConsumer(AuditService auditService) : IConsumer<CoverCreatedEvent>
{
    public async Task Consume(ConsumeContext<CoverCreatedEvent> context)
    {
        await auditService.AuditCover(context.Message.CoverId, context.Message.HttpRequestType);
    }
}
