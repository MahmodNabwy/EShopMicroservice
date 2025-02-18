
namespace Catalog.API.Products.DeleteProject;
//public record DeleteProductRequest(Guid Id);
public record DeleteProductResponse(bool IsSuccess);
public class DeleteProductEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{Id}", async (Guid Id, ISender sender) =>
        { 
            var result = await sender.Send(new DeleteProductCommand(Id));

            var response = result.Adapt<DeleteProductResponse>();

            return Results.Ok(response);

        }).WithName("DeleteProduct")
        .Produces<DeleteProductResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Product")
        .WithDescription("Delete Product");
    }
}

