﻿
using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.GetProductById;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Product Id Is Required");
        RuleFor(command => command.Name).NotEmpty().WithMessage("Name Is Required")
                                                   .Length(2, 150)
                                                   .WithMessage("Name must be between 2 and 150 characters");

        RuleFor(command => command.Category).NotEmpty().WithMessage("Category Is Required");
        RuleFor(command => command.ImageFile).NotEmpty().WithMessage("Image File Is Required");
        RuleFor(command => command.Price).GreaterThan(0).NotEmpty().WithMessage("Price must be greater than 0");
    }
}

internal class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IDocumentSession _session;

    public UpdateProductCommandHandler(IDocumentSession session)
    {
        _session = session;
    }
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {

        var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(command.Id);
        }

        product.Name = command.Name;
        product.Category = command.Category;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;

        _session.Update(product);
        await _session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(true);


    }
}

