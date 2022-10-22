using AutoMapper;
using new2me_api.Dtos;
using new2me_api.Models;

namespace new2me_api.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(){
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<Post, PostDtoWithoutContact>();
        }
    }
}