using Domain.Models;

namespace Application.Interfaces;

public interface IProjectService
{
    Task SubmitAsync(Guid id, PeriodDate periodDate);
}