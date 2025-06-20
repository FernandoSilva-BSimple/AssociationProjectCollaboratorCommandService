using Domain.Models;

namespace Application.Publishers;

public interface IMessagePublisher
{
    Task PublishOrderSubmittedAsync(Guid Id, Guid projectId, Guid collaboratorId, PeriodDate periodDate);
}
