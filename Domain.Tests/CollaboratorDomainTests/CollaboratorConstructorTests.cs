using Domain.Models;
using Moq;

namespace Domain.Tests.CollaboratorDomainTests;

public class CollaboratorConstructorTests
{

    [Fact]
    public void WhenCreatingCollaboratorWithId_ThenCollaboratorIsCreated()
    {
        //arrange
        Guid id = Guid.NewGuid(); ;

        //act & assert
        new Collaborator(id, It.IsAny<PeriodDateTime>());

    }
}