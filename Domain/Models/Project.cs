using Domain.Interfaces;

namespace Domain.Models;

public class Project : IProject
{
    public Guid Id { get; set; }

    public Project(Guid id)
    {
        Id = id;
    }
}
