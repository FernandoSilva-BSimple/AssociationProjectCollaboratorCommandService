using Domain.Models;

namespace Domain.Visitor;

public interface IProjectVisitor
{
    Guid Id { get; }
    PeriodDate PeriodDate { get; }
}
