using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GHDProductApi.IntegrationTests
{
    {
    public class ProductControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly IFixture _fixture;
        private readonly WebApplicationFactory<Program> _factory;

        public ProductControllerTest(IFixture fixture, WebApplicationFactory<Program> factory)
        {
            _fixture = fixture;
            _factory = factory;
        }
    }
}