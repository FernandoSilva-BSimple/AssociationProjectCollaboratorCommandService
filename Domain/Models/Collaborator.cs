using Domain.Interfaces;

namespace Domain.Models;

public class Collaborator : ICollaborator
{
    public Guid Id { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }

    public Collaborator(Guid id, PeriodDateTime periodDateTime)
    {
        Id = id;
        PeriodDateTime = periodDateTime;
    }
}
