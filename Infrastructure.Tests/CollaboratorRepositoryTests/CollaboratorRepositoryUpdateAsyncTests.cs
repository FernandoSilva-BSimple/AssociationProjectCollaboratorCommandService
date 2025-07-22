using System;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Infrastructure.Tests;

public class CollaboratorRepositoryEFTests : RepositoryTestBase
{
    [Fact]
    public async Task UpdateAsync_ShouldUpdatePeriodDates_WhenCollaboratorExists()
    {
        // Arrange
        var collaboratorId = Guid.NewGuid();
        var originalInitDate = new DateTime(2025, 1, 1);
        var originalFinalDate = new DateTime(2025, 1, 31);

        var newInitDate = new DateTime(2025, 2, 1);
        var newFinalDate = new DateTime(2025, 2, 28);

        var existingDataModel = new CollaboratorDataModel
        {
            Id = collaboratorId,
            PeriodDateTime = new PeriodDateTime(originalInitDate, originalFinalDate)
        };

        context.Set<CollaboratorDataModel>().Add(existingDataModel);
        await context.SaveChangesAsync();

        var repository = new CollaboratorRepositoryEF(context, _mapper.Object);

        var mockCollaborator = new Mock<ICollaborator>();
        mockCollaborator.Setup(c => c.Id).Returns(collaboratorId);
        mockCollaborator.Setup(c => c.PeriodDateTime).Returns(new PeriodDateTime(newInitDate, newFinalDate));

        // Act
        await repository.UpdateAsync(mockCollaborator.Object);

        // Assert
        var updated = await context.Set<CollaboratorDataModel>().FirstOrDefaultAsync(c => c.Id == collaboratorId);
        Assert.NotNull(updated);
        Assert.Equal(newInitDate, updated!.PeriodDateTime._initDate);
        Assert.Equal(newFinalDate, updated!.PeriodDateTime._finalDate);
    }
}
