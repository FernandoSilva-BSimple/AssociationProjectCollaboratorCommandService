using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using AutoMapper;

namespace Infrastructure.Tests;

public class AssociationProjectCollaboratorRepositoryEFTests : RepositoryTestBase
{
    [Fact]
    public async Task ExistsAsync_ReturnsTrue_WhenAssociationExists()
    {
        // Arrange
        var assocId = Guid.NewGuid();
        var assoc = new AssociationProjectCollaboratorDataModel
        {
            Id = assocId,
            CollaboratorId = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            PeriodDate = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10))
        };

        await context.Set<AssociationProjectCollaboratorDataModel>().AddAsync(assoc);
        await context.SaveChangesAsync();

        var repo = new AssociationProjectCollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.ExistsAsync(assocId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_ReturnsFalse_WhenAssociationDoesNotExist()
    {
        // Arrange
        var repo = new AssociationProjectCollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.ExistsAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ExistsWithCollaboratorAndProjectAndOverlappingPeriodAsync_ReturnsTrue_WhenOverlapExists()
    {
        // Arrange
        var collabId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var assoc = new AssociationProjectCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = collabId,
            ProjectId = projectId,
            PeriodDate = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10))
        };

        await context.Set<AssociationProjectCollaboratorDataModel>().AddAsync(assoc);
        await context.SaveChangesAsync();

        var overlappingPeriod = new PeriodDate(new DateOnly(2025, 7, 5), new DateOnly(2025, 7, 15));

        var repo = new AssociationProjectCollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.ExistsWithCollaboratorAndProjectAndOverlappingPeriodAsync(collabId, projectId, overlappingPeriod);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ExistsWithCollaboratorAndProjectAndOverlappingPeriodAsync_ReturnsFalse_WhenNoOverlap()
    {
        // Arrange
        var collabId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var assoc = new AssociationProjectCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = collabId,
            ProjectId = projectId,
            PeriodDate = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 10))
        };

        await context.Set<AssociationProjectCollaboratorDataModel>().AddAsync(assoc);
        await context.SaveChangesAsync();

        var nonOverlappingPeriod = new PeriodDate(new DateOnly(2025, 7, 11), new DateOnly(2025, 7, 20));

        var repo = new AssociationProjectCollaboratorRepositoryEF(context, _mapper.Object);

        // Act
        var result = await repo.ExistsWithCollaboratorAndProjectAndOverlappingPeriodAsync(collabId, projectId, nonOverlappingPeriod);

        // Assert
        Assert.False(result);
    }
}
