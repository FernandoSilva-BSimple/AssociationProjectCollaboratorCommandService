using Domain.Models;

namespace Domain.Messages;

public record ProjectCreatedMessage(Guid id, string title, string acronym, PeriodDate periodDate);
