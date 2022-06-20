using AutoMapper;
using week4.Data.Models;
using week4.Service.Dtos;


namespace week4.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<UserRefreshToken, ProductDto>().ReverseMap();


        }
    }
}
