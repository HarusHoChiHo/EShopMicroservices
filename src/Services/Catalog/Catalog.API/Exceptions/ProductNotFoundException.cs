using BuildingBlocks.Exceptions;

namespace CatalogAPI.Data.Exceptions;

public class ProductNotFoundException(Guid id) : NotFoundException("Product", id);