﻿using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name, List<string> Categories, string Description, string ImageFile, decimal Price);

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products",
            async (CreateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProductCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateProductResponse>();

            return Results.Created($"/product/{response.Id}", response);
        })
          .WithName("Create Product")
          .WithSummary("Creates a new product")
          .Produces<CreateProductResponse>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithDescription("Create Product");
    }
}

