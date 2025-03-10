


using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProject;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);
public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Product Id Is Required");        
    }
}
internal class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IDocumentSession _session;
 
    public DeleteProductCommandHandler(IDocumentSession session )
    {
        _session = session;
       
    }

    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
       

        var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product is null)
        {
            throw new ProductNotFoundException(command.Id);
        }

        _session.Delete(product);
        await _session.SaveChangesAsync();

        return new DeleteProductResult(true);
    }
}

