using AutoMapper;
using Blog.Shared.Entities;
using Blog.Shared.Models.ResponseModels;
using System.Collections.Generic;

namespace Blog.API.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           
            CreateMap<User, AuthResponseModel>().ReverseMap();
            CreateMap<User, UserResponseModel>().ReverseMap();
        }
    }
}
