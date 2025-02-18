


namespace Catalog.API.Products.DeleteProject;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);

internal class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IDocumentSession _session;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
    {
        _session = session;
        _logger = logger;
    }

    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeleteProductCommandHandler.Handle called with {@command}", command);

        var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product is null)
        {
            throw new ProductNotFoundException();
        }

        _session.Delete(product);
        await _session.SaveChangesAsync();

        return new DeleteProductResult(true);
    }
}

