using Alpha.api.Models;
using AutoMapper;

namespace Alpha.api.AutoMapper
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<PostProductVM, Product>();
            CreateMap<Product, PostProductVM>();
        }
    }
}
