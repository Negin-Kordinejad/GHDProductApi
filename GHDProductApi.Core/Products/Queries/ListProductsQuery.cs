using AutoMapper;
using FluentValidation;
using GHDProductApi.Core.Common.Models;
using GHDProductApi.Core.Products.Common.Models;
using GHDProductApi.Core.Responses;
using GHDProductApi.Infrastructure.Intefaces;
using MediatR;

namespace GHDProductApi.Core.Products.Queries
{
    public class ListProductsQuery : IRequest<Response<PaginatedDto<IEnumerable<ProductDto>>>>
    {
        public int PageNumber { get; set; }
        public int ItemsPerPage { get; set; } = 10;

        public class Validator : AbstractValidator<ListProductsQuery>
        {
            public Validator()
            {
                RuleFor(x => x.PageNumber)
                   .GreaterThan(0);
            }
        }

        public class Handler : IRequestHandler<ListProductsQuery, Response<PaginatedDto<IEnumerable<ProductDto>>>>
        {
            private readonly IProductService _productService;
            private readonly IMapper _mapper;

            /// <inheritdoc />
            public Handler(IProductService productService, IMapper mapper)
            {
                _productService = productService;
                _mapper = mapper;
            }

            public async Task<Response<PaginatedDto<IEnumerable<ProductDto>>>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
            {
                var response = new Response<PaginatedDto<IEnumerable<ProductDto>>>();
                var product = await _productService.GetPaginatedAsync(request.PageNumber, request.ItemsPerPage, cancellationToken);

                var totalProducts = await _productService.CountAsync(cancellationToken);

                var totalPage = (int)Math.Ceiling((double)totalProducts / request.ItemsPerPage);

                response.Data = new PaginatedDto<IEnumerable<ProductDto>>() { Data = product.Select(_mapper.Map<ProductDto>), HasNextPage = request.PageNumber < totalPage };
               
                return response;
            }
        }
    }
}
