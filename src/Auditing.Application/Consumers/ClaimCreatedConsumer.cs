using Claims.Events;
using MassTransit;

namespace Auditing.Application.Consumers;
internal sealed class ClaimCreatedConsumer(AuditService auditService) : IConsumer<ClaimCreatedEvent>
{
    public async Task Consume(ConsumeContext<ClaimCreatedEvent> context)
    {
        await auditService.AuditCover(context.Message.ClaimId, context.Message.HttpRequestType);
    }
}
