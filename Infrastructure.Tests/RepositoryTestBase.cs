using AutoMapper;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
namespace Infrastructure.Tests;

public class RepositoryTestBase
{
    protected readonly Mock<IMapper> _mapper;
    protected readonly AssociationDbContext context;

    protected RepositoryTestBase()
    {
        _mapper = new Mock<IMapper>();

        // Configure in-memory database
        var options = new DbContextOptionsBuilder<AssociationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
            .Options;

        context = new AssociationDbContext(options);
    }
}
