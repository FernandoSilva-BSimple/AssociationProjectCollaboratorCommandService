using Domain.Models;

namespace Application.Interfaces;

public interface ICollaboratorService
{
    Task SubmitAsync(Guid id, PeriodDateTime periodDateTime);
}
