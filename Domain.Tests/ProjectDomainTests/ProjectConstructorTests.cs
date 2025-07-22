using Domain.Models;
using Moq;

namespace Domain.Tests.ProjectDomainTests;

public class ProjectConstructorTests
{

    [Fact]
    public void WhenCreatingProjectWithId_ThenProjectIsCreated()
    {
        //arrange
        Guid id = Guid.NewGuid(); ;

        //act & assert
        new Project(id, It.IsAny<PeriodDate>());

    }
}