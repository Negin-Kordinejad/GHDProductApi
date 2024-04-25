using AutoMapper;
using GHDProductApi.Core.Common.Interfaces;
using GHDProductApi.Infrastructure.Entities;

namespace GHDProductApi.Core.Products.Common.Models
{
    public class ProductDto : IMapFrom<Product>
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public Money Price { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductDto>()
                   .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id));
        }

        public override string ToString()
        {
            return $"{Brand} {Name}";
        }
    }
}
