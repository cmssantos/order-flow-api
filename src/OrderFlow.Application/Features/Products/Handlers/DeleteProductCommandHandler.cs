using MediatR;
using OrderFlow.Application.Common.Models;
using OrderFlow.Application.Features.Products.Commands;
using OrderFlow.Domain.Interfaces.Repositories;

namespace OrderFlow.Application.Features.Products.Handlers;

public class DeleteProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<DeleteProductCommand, Result<Unit>>
{
    private readonly IProductRepository productRepository = productRepository;

    public async Task<Result<Unit>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(command.Id);
        if (product is null)
        {
            return Result<Unit>.Failure(
                "Product.NotFound",
                $"Product with ID {command.Id} not found."
            );
        }

        await productRepository.DeleteAsync(product.Id);

        return Result<Unit>.Success(Unit.Value);
    }
}
