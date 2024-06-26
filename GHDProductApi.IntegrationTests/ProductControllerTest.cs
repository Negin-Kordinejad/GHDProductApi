using FluentAssertions;
using GHDProductApi.Core.Products.Commands;
using GHDProductApi.Core.Products.Common.Models;
using GHDProductApi.Core.Responses;
using GHDProductApi.Infrastructure.Contexts;
using GHDProductApi.Infrastructure.Entities;
using GHDProductApi.IntegrationTests.Extensions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace GHDProductApi.IntegrationTests
{
    public class ProductControllerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient client;
        private const string AUS_CURRENCY_CODE = "AUD";

        public ProductControllerTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory
               .WithWebHostBuilder(builder =>
               {
                   builder.ConfigureServices(services =>
                   {
                       var sp = services.BuildServiceProvider();
                       using (var scope = sp.CreateScope())
                       {
                           var scopedServices = scope.ServiceProvider;
                           var appDb = scopedServices.GetRequiredService<ProductDbContext>();
                           appDb.Database.EnsureDeleted();
                           appDb.Database.EnsureCreated();
                       };
                   });
               });


            client = _factory.CreateClient();
        }

        [Fact]
        public async Task GivenProductId_1_WhenGettingProduct_ThenReturnP1()
        {
            // Arrange
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
                .Be(new Money(AUS_CURRENCY_CODE, 100m));

        }

        [Fact]
        public async Task GivenProductId100_WhenGettingProduct_Then_Return_UnSuccessfulResponse()
        {
            // Arrange
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
            var url = $"{client.BaseAddress}Product/Update";
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "PX1",
                Brand = "BX1",
                Currency = AUS_CURRENCY_CODE,
                Amount = 120m

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
                 .Be(new Money(AUS_CURRENCY_CODE, 120m));
        }

        [Fact]
        public async Task GivenProductId1_WhenUpdatingProduct_InvalidPrice_ThenReturnProduct()
        {
            // Arrange
            var url = $"{client.BaseAddress}Product/Update";
            var command = new UpdateProductCommand
            {
                Id = 1,
                Name = "PX1",
                Brand = "BX1",
                Currency = AUS_CURRENCY_CODE,
                Amount = 0
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
        public async Task GivenProduct_WhenAddingProduct_ThenReturnProduct()
        {
            // Arrange
            var url = $"{client.BaseAddress}Product/Add";
            var command = new CreateProductCommand
            {
                Name = "PX2",
                Brand = "BX2",
                Currency = AUS_CURRENCY_CODE,
                Amount = 1200m
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
                 .Be(new Money(AUS_CURRENCY_CODE, 1200m));
        }

        [Fact]
        public async Task GivenProduct_WhenAddingProduct_IFExists_Then_Return_UnSuccessfulResponse()
        {
            // Arrange
            var url = $"{client.BaseAddress}Product/Add";
            var command = new CreateProductCommand
            {
                Name = "P1",
                Brand = "B1",
                Currency = AUS_CURRENCY_CODE,
                Amount = 100m
            };

            string stringContent = JsonConvert.SerializeObject(command);

            // Act
            HttpResponseMessage responseMessage = await client.PostAsync(url, new StringContent(stringContent, Encoding.UTF8, MediaTypeNames.Application.Json));

            // Assert
            var productDto = await responseMessage.DeserializeContentAsync<Response<ProductDto>>();

            productDto.IsSuccessful.Should().BeFalse();
        }

        [Fact]
        public async Task GivenProduct_WhenDeleteingProduct_Then_ReturnDeleted()
        {
            // Arrange
            var url = $"{client.BaseAddress}Product?id=1";

            // Act
            var responseMessage = await client.DeleteAsync(url);


            // Assert
            var productDto = await responseMessage.DeserializeContentAsync<ProductDto>();

            productDto.ProductId.Should().Be(1);
        }
    }
}