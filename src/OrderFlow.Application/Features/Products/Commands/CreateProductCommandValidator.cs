using FluentValidation;
using OrderFlow.Domain;

namespace OrderFlow.Application.Features.Products.Commands;

public class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(FieldLengths.ProductNameMaxLength)
                .WithMessage("Product name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description is required.")
            .MaximumLength(FieldLengths.ProductDescriptionMaxLength)
                .WithMessage("Product description must not exceed 100 characters.");

        RuleFor(x => x.Sku)
            .NotEmpty().WithMessage("SKU is required.")
            .MaximumLength(FieldLengths.ProductSkuMaxLength)
                .WithMessage("SKU must not exceed 50 characters.");

        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");
    }
}
