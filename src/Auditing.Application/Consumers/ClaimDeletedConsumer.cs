using Claims.Events;
using MassTransit;

namespace Auditing.Application.Consumers;
internal sealed class ClaimDeletedonsumer(AuditService auditService) : IConsumer<ClaimDeletedEvent>
{
    public async Task Consume(ConsumeContext<ClaimDeletedEvent> context)
    {
        await auditService.AuditClaim(context.Message.ClaimId, context.Message.HttpRequestType);
    }
}
