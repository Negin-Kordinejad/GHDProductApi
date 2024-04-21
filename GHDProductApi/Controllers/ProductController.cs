using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace GHDProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IMediator mediator)
        {
            _mediator = mediator;
            _logger = logger;
        }

    }
}
