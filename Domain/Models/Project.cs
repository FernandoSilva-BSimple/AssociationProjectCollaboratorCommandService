using Domain.Interfaces;

namespace Domain.Models;

public class Project : IProject
{
    public Guid Id { get; set; }
    public PeriodDate PeriodDate { get; set; }

    public Project(Guid id, PeriodDate periodDate)
    {
        Id = id;
        PeriodDate = periodDate;
    }
}
