﻿

namespace Catalog.API.Products.GetProducts;


public record GetProductsQuery(int? PageNumber, int? PageSize) : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(IDocumentSession session)
                : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {

        var prodcuts = await session.Query<Product>()
                                    .ToPagedListAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);


        return new GetProductsResult(prodcuts);

    }
}

