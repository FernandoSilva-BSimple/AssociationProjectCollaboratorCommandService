using Application.DTO;
using Application.Publishers;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;
using Xunit;

namespace Application.Tests;

public class AssociationProjectCollaboratorServiceTests
{
    private readonly Mock<IAssociationProjectCollaboratorRepository> _repoMock = new();
    private readonly Mock<IAssociationProjectCollaboratorFactory> _factoryMock = new();
    private readonly Mock<IMessagePublisher> _publisherMock = new();

    private readonly AssociationProjectCollaboratorService _service;

    public AssociationProjectCollaboratorServiceTests()
    {
        _service = new AssociationProjectCollaboratorService(
            _repoMock.Object,
            _factoryMock.Object,
            _publisherMock.Object);
    }

    [Fact]
    public async Task Create_ReturnsSuccess_WhenValid()
    {
        // Arrange
        var dto = new CreateAssociationProjectCollaboratorDTO
        {
            ProjectId = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            PeriodDate = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10))
        };

        var entity = new AssociationProjectCollaborator(dto.ProjectId, dto.CollaboratorId, dto.PeriodDate);

        _factoryMock.Setup(f => f.Create(dto.ProjectId, dto.CollaboratorId, dto.PeriodDate.InitDate, dto.PeriodDate.FinalDate))
                    .ReturnsAsync(entity);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<IAssociationProjectCollaborator>()))
                 .ReturnsAsync(entity);

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(dto.ProjectId, result.Value.ProjectId);
        Assert.Equal(dto.CollaboratorId, result.Value.CollaboratorId);
        Assert.Equal(dto.PeriodDate, result.Value.PeriodDate);
        _publisherMock.Verify(p => p.PublishOrderSubmittedAsync(entity.Id, entity.ProjectId, entity.CollaboratorId, entity.PeriodDate), Times.Once);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenFactoryThrows()
    {
        // Arrange
        var dto = new CreateAssociationProjectCollaboratorDTO
        {
            ProjectId = Guid.NewGuid(),
            CollaboratorId = Guid.NewGuid(),
            PeriodDate = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10))
        };

        _factoryMock.Setup(f => f.Create(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateOnly>(), It.IsAny<DateOnly>()))
                    .ThrowsAsync(new ArgumentException("Invalid association"));

        // Act
        var result = await _service.Create(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Invalid association", result.Error.Message);
    }

    [Fact]
    public async Task CreateWithoutValidations_DoesNotAdd_WhenAlreadyExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repoMock.Setup(r => r.ExistsAsync(id)).ReturnsAsync(true);

        // Act
        await _service.CreateWithoutValidations(id, Guid.NewGuid(), Guid.NewGuid(), new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10)));

        // Assert
        _repoMock.Verify(r => r.AddAsync(It.IsAny<IAssociationProjectCollaborator>()), Times.Never);
    }

    [Fact]
    public async Task CreateWithoutValidations_Adds_WhenNotExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var collabId = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10));
        var entity = new AssociationProjectCollaborator(projectId, collabId, period);

        _repoMock.Setup(r => r.ExistsAsync(id)).ReturnsAsync(false);
        _factoryMock.Setup(f => f.Create(id, projectId, collabId, period)).Returns(entity);

        // Act
        await _service.CreateWithoutValidations(id, projectId, collabId, period);

        // Assert
        _repoMock.Verify(r => r.AddAsync(entity), Times.Once);
    }
}
