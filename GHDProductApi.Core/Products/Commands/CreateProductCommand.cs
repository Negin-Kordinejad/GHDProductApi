using AutoMapper;
using FluentValidation;
using GHDProductApi.Core.Products.Common.Models;
using GHDProductApi.Infrastructure.Entities;
using GHDProductApi.Infrastructure.Intefaces;
using MediatR;

namespace GHDProductApi.Core.Products.Commands
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public class Validator : AbstractValidator<CreateProductCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name)
                    .NotEmpty();

                RuleFor(x => x.Brand)
                    .NotEmpty();

                RuleFor(x => x.Price)
                    .GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<CreateProductCommand, ProductDto>
        {
            private readonly IProductService _productService;
            private readonly IMapper _mapper;

            public Handler(IProductService productService, IMapper mapper)
            {
                _productService = productService;
                _mapper = mapper;
            }

            public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {
                Product product = new()
                {
                    Name = request.Name,
                    Brand = request.Brand,
                    Price = request.Price
                };

                var addedProduct = await _productService.AddAsync(product, cancellationToken);
                var result = _mapper.Map<ProductDto>(addedProduct);

                return result;
            }
        }
    }
}
