using Domain.Factory;
using Domain.Models;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.CollaboratorDomainTests;

public class CollaboratorFactoryTests
{

    [Fact]
    public void WhenCreatingCollaborator_ThenCollaboratorIsCreated()
    {
        //arrange
        var CollaboratorFactory = new CollaboratorFactory();

        //act
        var Collaborator = CollaboratorFactory.Create(It.IsAny<Guid>(), It.IsAny<PeriodDateTime>());

        //assert
        Assert.NotNull(Collaborator);
    }

    [Fact]
    public void WhenCreatingCollaboratorFromVisitor_ThenCollaboratorIsCreated()
    {
        //arrange
        var CollaboratorFactory = new CollaboratorFactory();
        var CollaboratorVisitor = new Mock<ICollaboratorVisitor>();

        //act
        var Collaborator = CollaboratorFactory.Create(CollaboratorVisitor.Object);

        //assert
        Assert.NotNull(Collaborator);
    }
}