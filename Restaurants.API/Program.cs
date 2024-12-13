
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Application.Extensions;
using Serilog;
using Restaurants.API.Middleware;
using Restaurants.Domain.Entities;
using Restaurants.API.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.




builder.Services.AddApplication();
builder.AddPresentation();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IRestaurantSeeder>();


await seeder.Seed();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<TimerMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGroup("api/user")
    .WithTags("User")
    .MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

app.Run();
