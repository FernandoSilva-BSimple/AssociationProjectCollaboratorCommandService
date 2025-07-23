using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;
using Xunit;

namespace Application.Tests;

public class CollaboratorServiceTests
{
    private readonly Mock<ICollaboratorRepository> _repositoryMock = new();
    private readonly Mock<ICollaboratorFactory> _factoryMock = new();

    private readonly CollaboratorService _service;

    public CollaboratorServiceTests()
    {
        _service = new CollaboratorService(_repositoryMock.Object, _factoryMock.Object);
    }

    [Fact]
    public async Task SubmitAsync_CreatesAndAddsCollaborator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var period = new PeriodDateTime(new DateTime(2025, 7, 1), new DateTime(2025, 7, 31));
        var collaboratorMock = new Mock<ICollaborator>();

        _factoryMock.Setup(f => f.Create(id, period)).Returns(collaboratorMock.Object);

        // Act
        await _service.SubmitAsync(id, period);

        // Assert
        _factoryMock.Verify(f => f.Create(id, period), Times.Once);
        _repositoryMock.Verify(r => r.AddAsync(collaboratorMock.Object), Times.Once);
    }

    [Fact]
    public async Task SubmitUpdateAsync_CreatesAndUpdatesCollaborator()
    {
        // Arrange
        var id = Guid.NewGuid();
        var period = new PeriodDateTime(new DateTime(2025, 8, 1), new DateTime(2025, 8, 31));
        var collaboratorMock = new Mock<ICollaborator>();

        _factoryMock.Setup(f => f.Create(id, period)).Returns(collaboratorMock.Object);

        // Act
        await _service.SubmitUpdateAsync(id, period);

        // Assert
        _factoryMock.Verify(f => f.Create(id, period), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(collaboratorMock.Object), Times.Once);
    }
}
