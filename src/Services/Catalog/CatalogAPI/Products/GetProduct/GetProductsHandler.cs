namespace CatalogAPI.Products.GetProduct;

public struct GetProductQuery() : IQuery<GetProductResult>;
public struct GetProductResult(IEnumerable<Product> products);

internal class GetProductsHandler(IDocumentSession session) : IQueryHandler<GetProductQuery, GetProductResult>
{

    public Task<GetProductResult> Handle(GetProductQuery   request,
                                         CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}