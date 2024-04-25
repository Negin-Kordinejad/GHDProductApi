using AutoMapper;
using FluentValidation;
using GHDProductApi.Core.Products.Common.Models;
using GHDProductApi.Core.Responses;
using GHDProductApi.Infrastructure.Entities;
using GHDProductApi.Infrastructure.Intefaces;
using MediatR;

namespace GHDProductApi.Core.Products.Commands
{
    public class UpdateProductCommand : IRequest<ProductDto>
    {
        const string NotFoundMessageTemplate = "The product '{0}' could not be found.";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Currency { get; set; } = "AUD";
        public decimal Amount { get; set; }

        public class Validator : AbstractValidator<UpdateProductCommand>
        {

            private readonly IProductService _productService;

            public Validator(IProductService productService)
            {
                _productService = productService;

                RuleFor(x => x.Id)
                  .Custom((id, context) =>
                  {
                      if (!IsProductExists(id))
                      {
                          context.AddFailure("NotFoundFailure", string.Format(NotFoundMessageTemplate, id));
                      }
                  }).When(x => x.Id > 0);

                RuleFor(x => x.Id)
                    .GreaterThan(0);

                RuleFor(x => x.Name)
                   .NotEmpty();

                RuleFor(x => x.Brand)
                    .NotEmpty();

                RuleFor(x => x.Amount)
                    .GreaterThan(0);
            }

            private bool IsProductExists(int id)
            {
                var product = Task.Run(() => _productService.GetAsync(id)).Result;
                return product != null;
            }
        }
        public class Handler : IRequestHandler<UpdateProductCommand, ProductDto>
        {
            /// <inheritdoc />
            private readonly IProductService _productService;
            private readonly IMapper _mapper;

            public Handler(IProductService productService, IMapper mapper)
            {
                _productService = productService;
                _mapper = mapper;
            }

            public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {

                var product = await _productService.GetAsync(request.Id, cancellationToken);

                if (product == null)
                {
                    throw new NotFoundException(string.Format(NotFoundMessageTemplate, request.Id));
                }

                product.Name = request.Name;
                product.Brand = request.Brand;
                product.Price = new Money(request.Currency, request.Amount);

                var updatedProduct = await _productService.UpdateAsync(product, cancellationToken);

                return _mapper.Map<ProductDto>(updatedProduct);

            }
        }
    }
}
