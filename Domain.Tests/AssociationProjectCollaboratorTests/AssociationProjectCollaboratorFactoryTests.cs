using Domain.Factory;
using Domain.IRepository;
using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;
using Moq;

namespace Domain.Tests.AssociationProjectCollaboratorTests;

public class AssociationProjectCollaboratorFactoryTests
{
    [Fact]
    public async Task WhenCreatingAssociationWithValidData_ThenAssociationIsCreated()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();

        var period = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20));
        var collabPeriod = new PeriodDateTime(new DateTime(2025, 7, 1), new DateTime(2025, 7, 31));
        var projectPeriod = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 31));

        var collaboratorMock = new Mock<ICollaborator>();
        collaboratorMock.Setup(c => c.PeriodDateTime).Returns(collabPeriod);

        var projectMock = new Mock<IProject>();
        projectMock.Setup(p => p.PeriodDate).Returns(projectPeriod);

        var collabRepo = new Mock<ICollaboratorRepository>();
        collabRepo.Setup(r => r.GetByIdAsync(collaboratorId)).ReturnsAsync(collaboratorMock.Object);

        var projectRepo = new Mock<IProjectRepository>();
        projectRepo.Setup(r => r.GetByIdAsync(projectId)).ReturnsAsync(projectMock.Object);

        var assocRepo = new Mock<IAssociationProjectCollaboratorRepository>();
        assocRepo.Setup(r => r.ExistsWithCollaboratorAndProjectAndOverlappingPeriodAsync(collaboratorId, projectId, period)).ReturnsAsync(false);

        var factory = new AssociationProjectCollaboratorFactory(collabRepo.Object, projectRepo.Object, assocRepo.Object);

        // Act
        var result = await factory.Create(projectId, collaboratorId, period.InitDate, period.FinalDate);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(projectId, result.ProjectId);
        Assert.Equal(collaboratorId, result.CollaboratorId);
        Assert.Equal(period.InitDate, result.PeriodDate.InitDate);
        Assert.Equal(period.FinalDate, result.PeriodDate.FinalDate);
    }

    [Fact]
    public async Task WhenCollaboratorDoesNotExist_ThenThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();

        var collabRepo = new Mock<ICollaboratorRepository>();
        collabRepo.Setup(r => r.GetByIdAsync(collaboratorId)).ReturnsAsync((ICollaborator?)null);

        var projectRepo = new Mock<IProjectRepository>();
        var assocRepo = new Mock<IAssociationProjectCollaboratorRepository>();

        var factory = new AssociationProjectCollaboratorFactory(collabRepo.Object, projectRepo.Object, assocRepo.Object);

        var period = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20));

        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            factory.Create(projectId, collaboratorId, period.InitDate, period.FinalDate));

        Assert.Equal("Collaborator cannot be null.", ex.Message);
    }

    [Fact]
    public async Task WhenProjectDoesNotExist_ThenThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();

        var collabPeriod = new PeriodDateTime(new DateTime(2025, 7, 1), new DateTime(2025, 7, 31));
        var collaboratorMock = new Mock<ICollaborator>();
        collaboratorMock.Setup(c => c.PeriodDateTime).Returns(collabPeriod);

        var collabRepo = new Mock<ICollaboratorRepository>();
        collabRepo.Setup(r => r.GetByIdAsync(collaboratorId)).ReturnsAsync(collaboratorMock.Object);

        var projectRepo = new Mock<IProjectRepository>();
        projectRepo.Setup(r => r.GetByIdAsync(projectId)).ReturnsAsync((IProject?)null);

        var assocRepo = new Mock<IAssociationProjectCollaboratorRepository>();

        var factory = new AssociationProjectCollaboratorFactory(collabRepo.Object, projectRepo.Object, assocRepo.Object);

        var period = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20));

        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            factory.Create(projectId, collaboratorId, period.InitDate, period.FinalDate));

        Assert.Equal("Project cannot be null.", ex.Message);
    }

    [Fact]
    public async Task WhenAssociationOutsideCollaboratorPeriod_ThenThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();

        var collabPeriod = new PeriodDateTime(new DateTime(2025, 7, 1), new DateTime(2025, 7, 5)); // curtos
        var collaboratorMock = new Mock<ICollaborator>();
        collaboratorMock.Setup(c => c.PeriodDateTime).Returns(collabPeriod);

        var collabRepo = new Mock<ICollaboratorRepository>();
        collabRepo.Setup(r => r.GetByIdAsync(collaboratorId)).ReturnsAsync(collaboratorMock.Object);

        var projectRepo = new Mock<IProjectRepository>();
        var assocRepo = new Mock<IAssociationProjectCollaboratorRepository>();

        var factory = new AssociationProjectCollaboratorFactory(collabRepo.Object, projectRepo.Object, assocRepo.Object);

        var period = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 15));

        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            factory.Create(projectId, collaboratorId, period.InitDate, period.FinalDate));

        Assert.Equal("Association period must be within the collaborator's active period.", ex.Message);
    }

    [Fact]
    public async Task WhenAssociationOutsideProjectPeriod_ThenThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();

        var collabPeriod = new PeriodDateTime(new DateTime(2025, 7, 1), new DateTime(2025, 7, 31));
        var projectPeriod = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 15)); // termina mais cedo

        var collaboratorMock = new Mock<ICollaborator>();
        collaboratorMock.Setup(c => c.PeriodDateTime).Returns(collabPeriod);

        var projectMock = new Mock<IProject>();
        projectMock.Setup(p => p.PeriodDate).Returns(projectPeriod);

        var collabRepo = new Mock<ICollaboratorRepository>();
        collabRepo.Setup(r => r.GetByIdAsync(collaboratorId)).ReturnsAsync(collaboratorMock.Object);

        var projectRepo = new Mock<IProjectRepository>();
        projectRepo.Setup(r => r.GetByIdAsync(projectId)).ReturnsAsync(projectMock.Object);

        var assocRepo = new Mock<IAssociationProjectCollaboratorRepository>();

        var factory = new AssociationProjectCollaboratorFactory(collabRepo.Object, projectRepo.Object, assocRepo.Object);

        var period = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20));

        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            factory.Create(projectId, collaboratorId, period.InitDate, period.FinalDate));

        Assert.Equal("Association period must be within the project's active period.", ex.Message);
    }

    [Fact]
    public async Task WhenAssociationOverlapsWithExisting_ThenThrowsArgumentException()
    {
        var projectId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();

        var period = new PeriodDate(new DateOnly(2025, 7, 10), new DateOnly(2025, 7, 20));
        var collabPeriod = new PeriodDateTime(new DateTime(2025, 7, 1), new DateTime(2025, 7, 31));
        var projectPeriod = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 31));

        var collaboratorMock = new Mock<ICollaborator>();
        collaboratorMock.Setup(c => c.PeriodDateTime).Returns(collabPeriod);

        var projectMock = new Mock<IProject>();
        projectMock.Setup(p => p.PeriodDate).Returns(projectPeriod);

        var collabRepo = new Mock<ICollaboratorRepository>();
        collabRepo.Setup(r => r.GetByIdAsync(collaboratorId)).ReturnsAsync(collaboratorMock.Object);

        var projectRepo = new Mock<IProjectRepository>();
        projectRepo.Setup(r => r.GetByIdAsync(projectId)).ReturnsAsync(projectMock.Object);

        var assocRepo = new Mock<IAssociationProjectCollaboratorRepository>();
        assocRepo.Setup(r =>
            r.ExistsWithCollaboratorAndProjectAndOverlappingPeriodAsync(
                collaboratorId,
                projectId,
                It.Is<PeriodDate>(p =>
                    p.InitDate == period.InitDate &&
                    p.FinalDate == period.FinalDate))
        ).ReturnsAsync(true);


        var factory = new AssociationProjectCollaboratorFactory(collabRepo.Object, projectRepo.Object, assocRepo.Object);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            factory.Create(projectId, collaboratorId, period.InitDate, period.FinalDate));

        Assert.Equal("Association period for this collaborator and project must not overlap with another association.", ex.Message);
    }

    [Fact]
    public void WhenCreatingFromVisitor_ThenAssociationIsCreatedCorrectly()
    {
        // Arrange
        var visitorMock = new Mock<IAssociationProjectCollaboratorVisitor>();
        var id = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var collaboratorId = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10));

        visitorMock.Setup(v => v.Id).Returns(id);
        visitorMock.Setup(v => v.ProjectId).Returns(projectId);
        visitorMock.Setup(v => v.CollaboratorId).Returns(collaboratorId);
        visitorMock.Setup(v => v.PeriodDate).Returns(period);

        var factory = new AssociationProjectCollaboratorFactory(Mock.Of<ICollaboratorRepository>(), Mock.Of<IProjectRepository>(), Mock.Of<IAssociationProjectCollaboratorRepository>());

        // Act
        var assoc = factory.Create(visitorMock.Object);

        // Assert
        Assert.Equal(id, assoc.Id);
        Assert.Equal(projectId, assoc.ProjectId);
        Assert.Equal(collaboratorId, assoc.CollaboratorId);
        Assert.Equal(period, assoc.PeriodDate);
    }
}
