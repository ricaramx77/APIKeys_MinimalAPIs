namespace APIKeys_MinimalAPIs
{
    public static class ProductsEndpoints
    {
        public static WebApplication MapProductsEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/products").RequireAuthorization();

            group.MapGet("/{id}", GetProduct);

            return app;
        }

        public static GetProductDto GetProduct(int id) => new GetProductDto(id, "Watch");
    }

    public record GetProductDto(int Id, string Name);
}
