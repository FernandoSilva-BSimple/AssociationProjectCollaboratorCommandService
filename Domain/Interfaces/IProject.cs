using Domain.Models;

public interface IProject
{
    Guid Id { get; }
    PeriodDate PeriodDate { get; }
}