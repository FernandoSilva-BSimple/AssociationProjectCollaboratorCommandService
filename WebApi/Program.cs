using Application.DTO;
using Application.Interfaces;
using Application.Publishers;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Resolvers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using WebApi.Consumers;
using WebApi.Publishers;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName;

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Controllers
builder.Services.AddControllers();

// DB Context
builder.Services.AddDbContext<AssociationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Services
builder.Services.AddTransient<AssociationProjectCollaboratorService>();
builder.Services.AddTransient<ICollaboratorService, CollaboratorService>();
builder.Services.AddTransient<IProjectService, ProjectService>();

// Factories
builder.Services.AddTransient<IAssociationProjectCollaboratorFactory, AssociationProjectCollaboratorFactory>();
builder.Services.AddTransient<ICollaboratorFactory, CollaboratorFactory>();
builder.Services.AddTransient<IProjectFactory, ProjectFactory>();

// Repositories
builder.Services.AddTransient<IAssociationProjectCollaboratorRepository, AssociationProjectCollaboratorRepositoryEF>();
builder.Services.AddTransient<ICollaboratorRepository, CollaboratorRepositoryEF>();
builder.Services.AddTransient<IProjectRepository, ProjectRepositoryEF>();

// Mappers
builder.Services.AddTransient<AssociationProjectCollaboratorDataModelConverter>();
builder.Services.AddTransient<CollaboratorDataModelConverter>();
builder.Services.AddTransient<ProjectDataModelConverter>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<DataModelMappingProfile>();
    cfg.CreateMap<AssociationProjectCollaborator, AssociationProjectCollaboratorDTO>();
});

// Messaging
builder.Services.AddScoped<IMessagePublisher, MassTransitPublisher>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CollaboratorCreatedConsumer>();
    x.AddConsumer<CollaboratorUpdatedConsumer>();
    x.AddConsumer<ProjectUpdatedConsumer>();
    x.AddConsumer<ProjectCreatedConsumer>();
    x.AddConsumer<AssociationProjectCollaboratorCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");
        var instance = InstanceInfo.InstanceId;
        cfg.ReceiveEndpoint($"associations-cmd-{instance}", e =>
        {
            e.ConfigureConsumer<CollaboratorCreatedConsumer>(context);
            e.ConfigureConsumer<ProjectCreatedConsumer>(context);
            e.ConfigureConsumer<AssociationProjectCollaboratorCreatedConsumer>(context);
            e.ConfigureConsumer<CollaboratorUpdatedConsumer>(context);
            e.ConfigureConsumer<ProjectUpdatedConsumer>(context);
        });
    });
});

// Swagger/OpenAPI
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

Console.WriteLine("CONN: " + builder.Configuration.GetConnectionString("DefaultConnection"));

if (app.Environment.IsDevelopment() || env.StartsWith("Instance", StringComparison.OrdinalIgnoreCase))
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .SetIsOriginAllowed(_ => true)
    .AllowCredentials());

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.Run();

public partial class Program { }
