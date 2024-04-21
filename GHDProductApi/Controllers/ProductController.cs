using GHDProductApi.Core.Common.Models;
using GHDProductApi.Core.Products.Commands;
using GHDProductApi.Core.Products.Common.Models;
using GHDProductApi.Core.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GHDProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductAsync(
          [FromQuery] GetProductQuery query,
          CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }


        [HttpGet("List")]
        [ProducesResponseType(typeof(PaginatedDto<IEnumerable<ProductDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ListProductsAsync(
            [FromQuery] ListProductsQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("Add")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateProductAsync(
            [FromBody] CreateProductCommand body,
            CancellationToken cancellationToken
            )
        {
            var result = await _mediator.Send(body, cancellationToken);

            return CreatedAtAction(nameof(GetProductAsync).Replace("Async", ""), new { id = result.ProductId }, result);
        }

        [HttpPut("Update")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProductAsync(
            [FromBody] UpdateProductCommand body,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(body, cancellationToken);

            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteProductAsync(
            [FromQuery] DeleteProductCommand query,
            CancellationToken cancellationToken
            )
        {
            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}

