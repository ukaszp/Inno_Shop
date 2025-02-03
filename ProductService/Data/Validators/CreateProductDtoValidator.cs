using FluentValidation;
using ProductService.Data.DTOs;

namespace ProductService.Data.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name can have a maximum of 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Product description is required.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");
        }

    }
}
