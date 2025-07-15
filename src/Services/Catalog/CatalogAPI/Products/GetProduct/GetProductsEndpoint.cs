using CatalogAPI.Data.Models;

namespace CatalogAPI.Data.Products.GetProduct;

public record GetProductRequest();

public record GetProductResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products",
                   async (ISender sender) =>
                   {
                        var result = await sender.Send(new GetProductsQuery());
                        
                        var response = result.Adapt<GetProductResponse>();
                        
                        return Results.Ok(response);
                   })
           .WithName("GetProducts")
           .Produces<GetProductResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Products")
           .WithDescription("Get Products");
    }
}