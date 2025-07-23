using System.Net.Http.Json;
using Application.DTO;
using Domain.Models;
using Infrastructure.DataModel;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using WebApi.IntegrationTests.Tests;
using Xunit;

namespace WebApi.IntegrationTests.Controllers;

public class AssociationProjectCollaboratorControllerTests
    : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    public AssociationProjectCollaboratorControllerTests(IntegrationTestsWebApplicationFactory<Program> factory)
        : base(factory.CreateClient())
    {
        _factory = factory;
    }

    [Fact]
    public async Task POST_Create_ReturnsSuccessAndCorrectData()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AssociationDbContext>();

        var collabId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        var collabStart = DateTime.UtcNow;
        var collabEnd = collabStart.AddDays(30);
        var projectStart = DateOnly.FromDateTime(DateTime.UtcNow);
        var projectEnd = projectStart.AddDays(30);

        db.Collaborators.Add(new CollaboratorDataModel
        {
            Id = collabId,
            PeriodDateTime = new PeriodDateTime
            {
                _initDate = collabStart,
                _finalDate = collabEnd
            }
        });

        db.Projects.Add(new ProjectDataModel
        {
            Id = projectId,
            PeriodDate = new PeriodDate
            {
                InitDate = projectStart,
                FinalDate = projectEnd
            }
        });

        await db.SaveChangesAsync();

        var dto = new CreateAssociationProjectCollaboratorDTO
        {
            CollaboratorId = collabId,
            ProjectId = projectId,
            PeriodDate = new PeriodDate(
                DateOnly.FromDateTime(collabStart.AddDays(5)),
                DateOnly.FromDateTime(collabStart.AddDays(15))
            )
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/associationsPC", dto);

        // Assert
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AssociationProjectCollaboratorDTO>();
        Assert.NotNull(result);
        Assert.Equal(dto.CollaboratorId, result!.CollaboratorId);
        Assert.Equal(dto.ProjectId, result.ProjectId);
        Assert.Equal(dto.PeriodDate.InitDate, result.PeriodDate.InitDate);
        Assert.Equal(dto.PeriodDate.FinalDate, result.PeriodDate.FinalDate);
    }

    [Fact]
    public async Task POST_Create_ReturnsBadRequest_WhenAssociationOverlaps()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AssociationDbContext>();

        var collabId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var start = DateOnly.FromDateTime(DateTime.UtcNow);
        var end = start.AddDays(30);

        db.Collaborators.Add(new CollaboratorDataModel
        {
            Id = collabId,
            PeriodDateTime = new PeriodDateTime
            {
                _initDate = DateTime.SpecifyKind(start.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc),
                _finalDate = DateTime.SpecifyKind(end.ToDateTime(TimeOnly.MaxValue), DateTimeKind.Utc)
            }
        });


        db.Projects.Add(new ProjectDataModel
        {
            Id = projectId,
            PeriodDate = new PeriodDate
            {
                InitDate = start,
                FinalDate = end
            }
        });

        db.Associations.Add(new AssociationProjectCollaboratorDataModel
        {
            Id = Guid.NewGuid(),
            CollaboratorId = collabId,
            ProjectId = projectId,
            PeriodDate = new PeriodDate
            {
                InitDate = start.AddDays(5),
                FinalDate = start.AddDays(15)
            }
        });

        await db.SaveChangesAsync();

        var overlappingDto = new CreateAssociationProjectCollaboratorDTO
        {
            CollaboratorId = collabId,
            ProjectId = projectId,
            PeriodDate = new PeriodDate(
                start.AddDays(10),
                start.AddDays(20)
            )
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/associationsPC", overlappingDto);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}
