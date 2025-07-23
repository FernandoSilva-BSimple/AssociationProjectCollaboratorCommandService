
using Domain.Models;

namespace Domain.Messages;

public record ProjectUpdatedMessage(Guid Id, string Title, string Acronym, PeriodDate PeriodDate);