using Catalog.API.Products.GetProductById;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Products.GetProductByCategory;


public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler : IQueryHandler<GetProductsByCategoryQuery, GetProductByCategoryResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<GetProductByCategoryQueryHandler> _logger;
    public GetProductByCategoryQueryHandler(IDocumentSession session, ILogger<GetProductByCategoryQueryHandler> logger)
    {
        _session = session;
        _logger = logger;
    }
    public async Task<GetProductByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetProductByCategoryQueryHandler.Handle called with {@Query}", query);

        var products = await _session.Query<Product>().Where(p => p.Category.Contains(query.Category)).ToListAsync();

        return new GetProductByCategoryResult(products);
    }
}

