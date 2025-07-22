using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using AutoMapper;

namespace Infrastructure.Tests;

public class ProjectRepositoryEFTests : RepositoryTestBase
{
    [Fact]
    public async Task UpdateAsync_UpdatesPeriodDate_WhenProjectExists()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        var initialPeriod = new PeriodDate(new DateOnly(2025, 7, 1), new DateOnly(2025, 7, 15));
        var updatedPeriod = new PeriodDate(new DateOnly(2025, 8, 1), new DateOnly(2025, 8, 31));

        var existing = new ProjectDataModel
        {
            Id = projectId,
            PeriodDate = new PeriodDate(initialPeriod.InitDate, initialPeriod.FinalDate)
        };

        await context.Set<ProjectDataModel>().AddAsync(existing);
        await context.SaveChangesAsync();

        var projectMock = new Mock<IProject>();
        projectMock.Setup(p => p.Id).Returns(projectId);
        projectMock.Setup(p => p.PeriodDate).Returns(updatedPeriod);

        var repo = new ProjectRepositoryEF(context, _mapper.Object);

        // Act
        await repo.UpdateAsync(projectMock.Object);

        // Assert
        var updated = await context.Set<ProjectDataModel>().FirstOrDefaultAsync(p => p.Id == projectId);

        Assert.NotNull(updated);
        Assert.Equal(updatedPeriod.InitDate, updated!.PeriodDate.InitDate);
        Assert.Equal(updatedPeriod.FinalDate, updated.PeriodDate.FinalDate);
    }
}
