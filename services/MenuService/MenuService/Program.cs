using MassTransit;
using MediatR;
using MenuService.Application.Commands;
using MenuService.Application.Queries;
using MenuService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Configure MassTransit
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("admin");
            h.Password("password");
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Menu API endpoints
app.MapGet("/api/menu-items", async (IMediator mediator, Guid? categoryId, bool? isAvailable) =>
{
    var query = new GetMenuItemsQuery { CategoryId = categoryId, IsAvailable = isAvailable };
    var result = await mediator.Send(query);
    return Results.Ok(result);
})
.WithName("GetMenuItems");

app.MapPost("/api/menu-items", async (IMediator mediator, CreateMenuItemCommand command) =>
{
    var result = await mediator.Send(command);
    return Results.Created($"/api/menu-items/{result.Id}", result);
})
.WithName("CreateMenuItem");

app.Run();
