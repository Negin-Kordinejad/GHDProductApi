using AutoMapper;
using FluentValidation;
using GHDProductApi.Core.Products.Common.Models;
using GHDProductApi.Core.Responses;
using GHDProductApi.Infrastructure.Entities;
using GHDProductApi.Infrastructure.Intefaces;
using MediatR;

namespace GHDProductApi.Core.Products.Queries
{
    public class GetProductQuery : IRequest<Response<ProductDto>>
    {
        public int Id { get; set; }

        public class Validator : AbstractValidator<GetProductQuery>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<GetProductQuery, Response<ProductDto>>
        {
            private readonly IProductService _productService;
            private readonly IMapper _mapper;

            public Handler(IProductService productService, IMapper mapper)
            {
                _productService = productService;
                _mapper = mapper;
            }

            public async Task<Response<ProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
            {
                var response = new Response<ProductDto>();

                Product? product = await _productService.GetAsync(request.Id, cancellationToken);

                if (product is default(Product))
                {
                    response.AddError(ResponseErrorCodeConstants.NotFoundException, $"The product '{request.Id}' could not be found.");
                    return response;
                }

                response.Data = _mapper.Map<ProductDto>(product);

                return response;
            }

        }
    }
}
