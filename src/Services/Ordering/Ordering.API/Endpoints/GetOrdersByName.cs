using Ordering.Application.Orders.Queries.GetOrderByName;

namespace Ordering.API.Endpoints;

//public record GetOrdersByNameRequest(string name);

public record GetOrdersByNameResult(IEnumerable<OrderDto> Orders);

public class GetOrdersByName : ICarterModule
{

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/orders/{orderName}",
                   async (string  orderName,
                          ISender sender) =>
                   {
                       var query = await sender.Send(new GetOrdersByNameQuery(orderName));

                       var response = query.Adapt<GetOrdersByNameResult>();
                       
                       return Results.Ok(response);
                   })
           .WithName("GetOrdersByName")
           .Produces<GetOrdersByNameResult>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .WithSummary("Get Orders By Name")
           .WithDescription("Get Orders By Name");
    }
}