using MassTransit;
using MediatR;
using KitchenService.Application.Commands;
using KitchenService.Application.Queries;
using KitchenService.Domain.Entities;
using KitchenService;
using KitchenService.Application.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Configure MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

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

// Kitchen API endpoints
app.MapGet("/api/kitchen-orders", async (IMediator mediator, KitchenOrderStatus? status) =>
{
    var query = new GetKitchenOrdersQuery { Status = status };
    var result = await mediator.Send(query);
    return Results.Ok(result);
})
.WithName("GetKitchenOrders");

app.MapPut("/api/kitchen-orders/{orderId:guid}/status", async (IMediator mediator, Guid orderId, UpdateOrderStatusRequest request) =>
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

public record UpdateOrderStatusRequest(KitchenOrderStatus Status, string? Notes);
