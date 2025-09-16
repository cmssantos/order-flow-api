using FluentValidation;

namespace OrderFlow.Application.Features.Products.Commands;

public class AdjustProductStockCommandValidator : AbstractValidator<AdjustProductStockCommand>
{
    public AdjustProductStockCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.Adjustment)
            .NotEqual(0).WithMessage("Stock adjustment must not be zero.")
            .GreaterThan(-1000).WithMessage("Stock adjustment is too negative.")
            .LessThan(1000).WithMessage("Stock adjustment is too large.");
    }
}
