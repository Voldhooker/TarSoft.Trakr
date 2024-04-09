using Asp.Versioning;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TarSoft.GpsUnit.Api.CQRS.Queries;
using TarSoft.GpsUnit.Domain;
using TarSoft.GpsUnit.Infrastructure;
using TarSoft.Mediator;
using TarSoft.Mediator.Behaviours;

var builder = WebApplication.CreateBuilder(args);





/************** Tarsoft.Mediator*/
builder.Services.TryAddSingleton<ICommandDispatcher, CommandDispatcher>();
builder.Services.TryAddSingleton<IQueryDispatcher, QueryDispatcher>();
builder.Services.AddScoped<IMediator, SimpleMediator>();

//builder.Services.AddScoped<IQueryHandler<GetAllGpsUnitsQuery, Result<List<GpsUnit>>>, GetAllGpsUnitsHandler>();


// INFO: Using https://www.nuget.org/packages/Scrutor for registering all Query and Command handlers by convention
//builder.Services.Scan(selector =>
//{
//    selector.FromCallingAssembly()
//            .AddClasses(filter =>
//            {
//                filter.AssignableTo(typeof(IQueryHandler<,>));
//            })
//            .AsImplementedInterfaces()
//            .WithScopedLifetime();
//});
//builder.Services.Scan(selector =>
//{
//    selector.FromCallingAssembly()
//            .AddClasses(filter =>
//            {
//                filter.AssignableTo(typeof(ICommandHandler<,>));
//            })
//            .AsImplementedInterfaces()
//            .WithScopedLifetime();
//});

builder.Services.Scan(scan => scan
    .FromCallingAssembly()
    .AddClasses(classes => classes.AssignableToAny(
        typeof(IQueryHandler<,>),
        typeof(ICommandHandler<,>)
    ))
    .AsImplementedInterfaces()
    .WithScopedLifetime());

builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

/*******************/

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0); // Default version
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Set up DbContext
//builder.Services.AddDbContext<GpsUnitContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//});

builder.Services.AddDbContext<GpsUnitContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
