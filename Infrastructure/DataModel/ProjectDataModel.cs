using Domain.Visitor;

public class ProjectDataModel : IPRojectVisitor
{
    public Guid Id { get; set; }
    public ProjectDataModel() { }

    public ProjectDataModel(IProject project)
    {
        Id = project.Id;
    }
}