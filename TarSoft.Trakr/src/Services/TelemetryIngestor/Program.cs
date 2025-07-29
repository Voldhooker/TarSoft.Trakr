using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TarSoft.Mediator;
using TelemetryIngestor.Common.Behaviors;
using TelemetryIngestor.Infrastructure;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<TelemetryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                        "Server=(localdb)\\mssqllocaldb;Database=TelemetryIngestor;Trusted_Connection=true;MultipleActiveResultSets=true"));

// Mediator
builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();
builder.Services.AddScoped<IMediator, SimpleMediator>();

// Register all handlers from the current assembly
var assembly = Assembly.GetExecutingAssembly();

// Command handlers
foreach (var type in assembly.GetTypes())
{
    var interfaces = type.GetInterfaces()
        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))
        .ToList();

    foreach (var @interface in interfaces)
    {
        builder.Services.AddScoped(@interface, type);
    }
}

// Query handlers
foreach (var type in assembly.GetTypes())
{
    var interfaces = type.GetInterfaces()
        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))
        .ToList();

    foreach (var @interface in interfaces)
    {
        builder.Services.AddScoped(@interface, type);
    }
}

// FluentValidation
builder.Services.AddValidatorsFromAssembly(assembly);

// Pipeline behaviors
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TelemetryDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
