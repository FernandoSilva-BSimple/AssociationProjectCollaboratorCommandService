using Domain.Factory;
using Domain.Models;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.ProjectDomainTests;

public class ProjectFactoryTests
{

    [Fact]
    public void WhenCreatingProject_ThenProjectIsCreated()
    {
        //arrange
        var projectFactory = new ProjectFactory();

        //act
        var project = projectFactory.Create(It.IsAny<Guid>(), It.IsAny<PeriodDate>());

        //assert
        Assert.NotNull(project);
    }

    [Fact]
    public void WhenCreatingProjectFromVisitor_ThenProjectIsCreated()
    {
        //arrange
        var projectFactory = new ProjectFactory();
        var projectVisitor = new Mock<IProjectVisitor>();

        //act
        var Project = projectFactory.Create(projectVisitor.Object);

        //assert
        Assert.NotNull(Project);
    }
}