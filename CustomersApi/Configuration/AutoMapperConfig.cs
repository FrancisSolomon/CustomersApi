using AutoMapper;

using CustomersApi.Models;
using CustomersApi.V1.Models;

namespace CustomersApi.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            MapCustomer();
            MapContact();
            MapAddress();
        }

        private void MapCustomer()
        {
            CreateMap<Customer, CustomerDto>()
                .ForMember(d => d.CustomerId, s => s.MapFrom(src => src.Id))
                .ReverseMap();
        }

        private void MapContact()
        {
            CreateMap<Contact, ContactDto>()
                .ForMember(d => d.ContactId, s => s.MapFrom(src => src.Id))
                .ReverseMap();
        }

        private void MapAddress()
        {
            CreateMap<Address, AddressDto>()
                .ForMember(d => d.AddressId, s => s.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
