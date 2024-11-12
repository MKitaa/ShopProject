using AutoMapper;
using Shop.DTOs.Cart;
using Shop.DTOs.product;
using Shop.Models;

namespace Shop.Services;

public class ShopMappingProfile : Profile
{
    public ShopMappingProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        CreateMap<ShoppingCartItem, ShoppingCartDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
    }
}
