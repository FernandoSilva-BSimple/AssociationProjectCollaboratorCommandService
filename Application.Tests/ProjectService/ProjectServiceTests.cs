using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;
using Xunit;

namespace Application.Tests;

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _repositoryMock = new();
    private readonly Mock<IProjectFactory> _factoryMock = new();

    private readonly ProjectService _service;

    public ProjectServiceTests()
    {
        _service = new ProjectService(_repositoryMock.Object, _factoryMock.Object);
    }

    [Fact]
    public async Task SubmitAsync_CreatesAndAddsProject()
    {
        // Arrange
        var id = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 31));
        var projectMock = new Mock<IProject>();

        _factoryMock.Setup(f => f.Create(id, period)).Returns(projectMock.Object);

        // Act
        await _service.SubmitAsync(id, period);

        // Assert
        _factoryMock.Verify(f => f.Create(id, period), Times.Once);
        _repositoryMock.Verify(r => r.AddAsync(projectMock.Object), Times.Once);
    }

    [Fact]
    public async Task SubmitUpdateAsync_CreatesAndUpdatesProject()
    {
        // Arrange
        var id = Guid.NewGuid();
        var period = new PeriodDate(new DateOnly(2025, 8, 1), new DateOnly(2025, 8, 31));
        var projectMock = new Mock<IProject>();

        _factoryMock.Setup(f => f.Create(id, period)).Returns(projectMock.Object);

        // Act
        await _service.SubmitUpdateAsync(id, period);

        // Assert
        _factoryMock.Verify(f => f.Create(id, period), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(projectMock.Object), Times.Once);
    }
}
