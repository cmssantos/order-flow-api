using FluentValidation;

namespace OrderFlow.Application.Features.Products.Commands;

public class AdjustProductStockCommandValidator: AbstractValidator<AdjustProductStockCommand>
{
    private const int neutralAdjustment = 0;

    public AdjustProductStockCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required.");

        RuleFor(x => x.Adjustment)
            .NotEqual(neutralAdjustment)
            .WithMessage("Adjustment value cannot be zero.");
    }
}
