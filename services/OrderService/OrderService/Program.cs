using MassTransit;
using MediatR;
using OrderService.Application.Commands;
using OrderService.Application.Queries;
using OrderService.Application.Sagas;
using OrderService.Application.Consumers;
using OrderService.Domain.Entities;
using OrderService;
using OrderService.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Configure MassTransit with Saga
builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<OrderProcessingStateMachine, OrderProcessingSagaState>()
        .EntityFrameworkRepository(r =>
        {
            r.ExistingDbContext<OrderDbContext>();
        });

    x.AddConsumer<OrderPlacedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq", "/", h =>
        {
            h.Username("admin");
            h.Password("password");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Order API endpoints
app.MapPost("/api/orders", async (IMediator mediator, CreateOrderCommand command) =>
{
    var result = await mediator.Send(command);
    return Results.Created($"/api/orders/{result.Id}", result);
})
.WithName("CreateOrder");

app.MapGet("/api/orders/{orderId:guid}", async (IMediator mediator, Guid orderId) =>
{
    var query = new GetOrderQuery { OrderId = orderId };
    var result = await mediator.Send(query);
    return Results.Ok(result);
})
.WithName("GetOrder");

app.MapPut("/api/orders/{orderId:guid}/status", async (IMediator mediator, Guid orderId, UpdateOrderStatusRequest request) =>
{
    var command = new UpdateOrderStatusCommand
    {
        OrderId = orderId,
        Status = request.Status,
        Notes = request.Notes
    };

    var result = await mediator.Send(command);
    return Results.Ok(result);
})
.WithName("UpdateOrderStatus");

app.Run();

public record UpdateOrderStatusRequest(OrderStatus Status, string? Notes);
