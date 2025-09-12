using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Products.Commands;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Products.Handlers;

public class DeleteProductCommandHandler(IUnitOfWork unitOfWork): IRequestHandler<DeleteProductCommand, Result<Unit>>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;

    public async Task<Result<Unit>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Products.GetByIdAsync(command.Id, cancellationToken);
        if (product is null)
        {
            return Result<Unit>.Failure(
                "Product.NotFound",
                $"Product with ID {command.Id} not found."
            );
        }

        await unitOfWork.Products.DeleteAsync(product.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Unit>.Success(Unit.Value);
    }
}
