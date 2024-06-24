using Claims.Events;
using MassTransit;

namespace Auditing.Application.Consumers;
internal sealed class CoverDeletedConsumer(AuditService auditService) : IConsumer<CoverDeletedEvent>
{
    public async Task Consume(ConsumeContext<CoverDeletedEvent> context)
    {
        await auditService.AuditCover(context.Message.CoverId, context.Message.HttpRequestType);
    }
}
