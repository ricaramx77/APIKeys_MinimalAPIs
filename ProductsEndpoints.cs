

using FluentValidation;
using MinimalApiFilters;
using System.ComponentModel.DataAnnotations;

namespace APIKeys_MinimalAPIs
{
    public static class ProductsEndpoints
    {
        public static WebApplication MapProductsEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/products");

            //group.MapGet("/{id}", GetProduct).AddFilter(new AuthorizationFilter(new[] { "Admin", "Manager" }));

            group.MapGet("/{id}", GetProduct).AddFilter<LoggingFilter>();

            //app.MapPost("/orders", (Order order) => Results.Ok(order)).AddFilter<ValidationFilter<Order>>();

            app.MapPost("/orders", (OrderFV order) => Results.Ok(order)).AddFilter<ValidationFilter<OrderFV>>();

            return app;
        }

        public static GetProductDto GetProduct(int id) => new GetProductDto(id, "Watch");
    }

    public record GetProductDto(int Id, string Name);
}

// Modelo de prueba
//public record Order(
//    [Required]
//    [StringLength(50, MinimumLength = 3)]
//    string Product,

//    [Required]
//    [Range(1, 100)]
//    int Quantity,

//    [EmailAddress]
//    string Email
//);


//public record Order
//{
//    [Required]
//    [StringLength(50, MinimumLength = 3)]
//    public string Product { get; init; }


//    [Required]
//    [Range(1, 100)]
//    public int Quantity { get; init; }

//    [EmailAddress]
//    public string Email { get; init; }
//}

public class OrderValidator : AbstractValidator<OrderFV>
{
    public OrderValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo es obligatorio.")
            .EmailAddress().WithMessage("El formato del correo no es válido.");

        RuleFor(o => o.Product)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(50).WithMessage("Product name must be at most 50 characters.");

        RuleFor(x => x.Quantity)
           .InclusiveBetween(1, 100)
           .WithMessage("La cantidad debe estar entre 1 y 100.");
    }
}

public record OrderFV
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Product { get; init; }


    [Required]
    [Range(1, 100)]
    public int Quantity { get; init; }

    [EmailAddress]
    public string Email { get; init; }
}




