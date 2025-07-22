using Domain.Models;
using Moq;

namespace Domain.Tests.AssociationProjectCollaboratorTests;

public class AssociationProjectCollaboratorConstructorTests
{
    [Fact]
    public void WhenCreatingAssociationWithId_ThenAssociationIsCreated()
    {
        //arrange

        //act & assert
        new AssociationProjectCollaborator(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>());
    }

    [Fact]
    public void WhenCreatingAssociation_ThenAssociationIsCreated()
    {
        //arrange

        //act & assert
        new AssociationProjectCollaborator(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PeriodDate>());
    }
}