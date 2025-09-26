using FluentValidation;

namespace OrderFlow.Application.Features.Products.Commands;

public class UpdateProductPriceCommandValidator: AbstractValidator<UpdateProductPriceCommand>
{
    private const decimal neutralPrice = 0m;

    public UpdateProductPriceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.Price)
            .GreaterThan(neutralPrice)
            .WithMessage("Price must be greater than zero.");
    }
}
