using AutoMapper;
using FluentValidation;
using GHDProductApi.Core.Products.Common.Models;
using GHDProductApi.Core.Responses;
using GHDProductApi.Infrastructure.Entities;
using GHDProductApi.Infrastructure.Intefaces;
using MediatR;

namespace GHDProductApi.Core.Products.Commands
{
    public class DeleteProductCommand : IRequest<ProductDto>
    {
        public int Id { get; set; }

        public class Validator : AbstractValidator<DeleteProductCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<DeleteProductCommand, ProductDto>
        {
            private readonly IProductService _productService;
            private readonly IMapper _mapper;

            public Handler(IProductService productService, IMapper mapper)
            {
                _productService = productService;
                _mapper = mapper;
            }

            /// <inheritdoc />
            public async Task<ProductDto> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var deletedProduct = await _productService.DeleteAsync(request.Id, cancellationToken);

                if (deletedProduct is default(Product)) throw new NotFoundException($"The product '{request.Id}' could not be found.");

                return _mapper.Map<ProductDto>(deletedProduct);
            }
        }
    }
}
