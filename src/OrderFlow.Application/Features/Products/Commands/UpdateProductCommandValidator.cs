using FluentValidation;
using OrderFlow.Domain;

namespace OrderFlow.Application.Features.Products.Commands;

public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand>
{
    private const int nameMinLength = FieldLengths.ProductNameMinLength;
    private const int nameMaxLength = FieldLengths.ProductNameMaxLength;
    private const int descriptionMinLength = FieldLengths.ProductDescriptionMinLength;
    private const int descriptionMaxLength = FieldLengths.ProductDescriptionMaxLength;

    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MinimumLength(nameMinLength)
                .WithMessage($"Product name must be at least {nameMinLength} characters.")
            .MaximumLength(nameMaxLength)
                .WithMessage($"Product name must not exceed {nameMaxLength} characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Product description is required.")
            .MinimumLength(descriptionMinLength)
                .WithMessage($"Product description must be at least {descriptionMinLength} characters.")
            .MaximumLength(descriptionMaxLength)
                .WithMessage($"Product description must not exceed {descriptionMaxLength} characters.");
    }
}
