using FluentAssertions;
using GHDProductApi.Core.Products.Commands;
using GHDProductApi.Core.Products.Common.Models;
using GHDProductApi.Core.Responses;
using GHDProductApi.IntegrationTests.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace GHDProductApi.IntegrationTests
{
    public class ProductControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProductControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GivenProductId1_WhenGettingProduct_ThenReturnP1()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            var url = $"{client.BaseAddress}Product?id=1";

            // Act
            var responseMessage = await client.GetAsync(url);

            // Assert
            responseMessage.EnsureSuccessStatusCode();
            var productDto = await responseMessage.DeserializeContentAsync<Response<ProductDto>>();
            productDto.IsSuccessful.Should()
                .BeTrue();
            productDto.Data.Name.Should()
                .Be("P1");

            productDto.Data.Brand.Should()
                .Be("B1");

            productDto.Data.Price.Should()
                .Be(100);

        }

        [Fact]
        public async Task GivenProductId100_WhenGettingProduct_ThenReturnUnSuccessfulRespomce()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            var url = $"{client.BaseAddress}Product?id=100";

            // Act
            var responseMessage = await client.GetAsync(url);

            // Assert
            responseMessage.EnsureSuccessStatusCode();
            var productDto = await responseMessage.DeserializeContentAsync<Response<ProductDto>>();
            productDto.IsSuccessful.Should()
               .BeFalse();
            productDto.ErrorMessages[0].ErrorCode.Should()
              .Be("NotFoundError");
        }

        [Fact]
        public async Task GivenProductId1_WhenUpdatingProduct_ThenReturnProduct()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            var url = $"{client.BaseAddress}Product/Update";
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "PX1",
                Brand = "BX1",
                Price = 120
            };

            string stringContent = JsonConvert.SerializeObject(command);

            // Act
            HttpResponseMessage responseMessage = await client.PutAsync(url, new StringContent(stringContent, Encoding.UTF8, MediaTypeNames.Application.Json));

            // Assert
            var productDto = await responseMessage.DeserializeContentAsync<ProductDto>();

            productDto.Name.Should()
                .Be("PX1");
            productDto.Brand.Should()
                .Be("BX1");
            productDto.Price.Should()
                .Be(120);
        }

        [Fact]
        public async Task GivenProductId1_WhenUpdatingProduct_InvalisPrice_ThenReturnProduct()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            var url = $"{client.BaseAddress}Product/Update";
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "PX1",
                Brand = "BX1",
                Price = 0
            };

            string stringContent = JsonConvert.SerializeObject(command);

            // Act
            HttpResponseMessage responseMessage = await client.PutAsync(url, new StringContent(stringContent, Encoding.UTF8, MediaTypeNames.Application.Json));

            // Assert
            var productDto = await responseMessage.DeserializeContentAsync<Response<ProductDto>>();
            productDto.IsSuccessful.Should()
                .BeFalse();
        }

        [Fact]
        public async Task GivenProductId1_WhenAddingProduct_ThenReturnProduct()
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            var url = $"{client.BaseAddress}Product/Add";
            var command = new CreateProductCommand
            {
                Name = "PX2",
                Brand = "BX2",
                Price = 1200
            };

            string stringContent = JsonConvert.SerializeObject(command);

            // Act
            HttpResponseMessage responseMessage = await client.PostAsync(url, new StringContent(stringContent, Encoding.UTF8, MediaTypeNames.Application.Json));

            // Assert
            var productDto = await responseMessage.DeserializeContentAsync<ProductDto>();

            productDto.Name.Should()
                .Be("PX2");
            productDto.Brand.Should()
                .Be("BX2");
            productDto.Price.Should()
                .Be(1200);
        }
    }
}