namespace CatalogAPI.Products.GetProduct;

public record GetProductRequest();
public record GetProductResponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products",
                   async (GetProductQuery query,
                          ISender         sender) =>
                   {
                       
                   })
           .WithName("GetProducts")
           .Produces<GetProductResponse>(StatusCodes.Status200OK)
           .ProducesProblem(StatusCodes.Status404NotFound)
           .WithSummary("Get Products")
           .WithDescription("Get Products");
    }
}