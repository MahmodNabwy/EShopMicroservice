﻿

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);

internal class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    public GetProductByIdQueryHandler(IDocumentSession session, ILogger<GetProductByIdQueryHandler> logger)
    {
        _session = session;
        _logger = logger;
    }
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _session.LoadAsync<Product>(query.Id, cancellationToken);
        if (product is null)
        {
            throw new ProductNotFoundException(query.Id);
        }
        return new GetProductByIdResult(product);
    }
}

