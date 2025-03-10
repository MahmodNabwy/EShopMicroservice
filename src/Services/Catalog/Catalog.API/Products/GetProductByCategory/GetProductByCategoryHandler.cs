using Catalog.API.Products.GetProductById;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Products.GetProductByCategory;


public record GetProductsByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler : IQueryHandler<GetProductsByCategoryQuery, GetProductByCategoryResult>
{
    private readonly IDocumentSession _session;
     public GetProductByCategoryQueryHandler(IDocumentSession session )
    {
        _session = session;
        
    }
    public async Task<GetProductByCategoryResult> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
 
        var products = await _session.Query<Product>().Where(p => p.Category.Contains(query.Category)).ToListAsync();

        return new GetProductByCategoryResult(products);
    }
}

