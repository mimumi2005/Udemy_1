using AutoMapper;
using Restaurants.Application.Restaurants.Commads.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.EditRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos
{
    public class RestaurantsProfile : Profile
    {
        public RestaurantsProfile()
        {

            CreateMap<CreateRestaurantCommand, Restaurant>().
                ForMember(d => d.Address, opt => opt.MapFrom(
                    src => new Address
                    {
                        City = src.City,
                        PostalCode = src.PostalCode,
                        Street = src.Street
                    }));

            CreateMap<EditRestaurantCommand, Restaurant>()
                .ForMember(d => d.Name, opt =>
                opt.MapFrom(src => src.Name))
                .ForMember(d => d.Description, opt =>
                opt.MapFrom(src => src.Description))
                .ForMember(d => d.HasDelivery, opt =>
                opt.MapFrom(src => src.HasDelivery))
                .ForMember(d => d.Id, opt =>
                opt.MapFrom(src => src.Id));



            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(d => d.City, opt =>
                opt.MapFrom(src => src.Address == null ? null : src.Address.City))
                .ForMember(d => d.City, opt =>
                opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
                .ForMember(d => d.City, opt =>
                opt.MapFrom(src => src.Address == null ? null : src.Address.Street))
                .ForMember(d => d.Dishes, opt =>
                opt.MapFrom(src => src.Dishes));
        }

    }
}
