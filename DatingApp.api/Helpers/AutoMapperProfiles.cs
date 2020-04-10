using AutoMapper;
using DatingApp.api.Models;
using DatingApp.api.DTOS;
using System.Collections.Generic;
using System.Linq;

namespace DatingApp.api.Helpers
{
    //Creat Map: tell which class map to which class
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            //formember -> 用來mapp名稱不同的property name
            CreateMap<User,UserForListDto>()
                .ForMember(dest=> dest.PhotoUrl, 
                    option=> option.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url))
                        .ForMember(x=>x.Age, y=>y.MapFrom(z=>z.DateOfBirth.CaculateYear()));
            CreateMap<User,UserForDetailedDTO>().
                ForMember(dest=> dest.PhotoUrl, 
                    option=> option.MapFrom(src=>src.Photos.FirstOrDefault(x=>x.IsMain).Url))
                        .ForMember(x=>x.Age, y=>y.MapFrom(z=>z.DateOfBirth.CaculateYear()));
            CreateMap<Photo,PhotoDetailedDto>();
            CreateMap<UserForUpdateDTO,User>(); 
            CreateMap<Photo,PhotoForReturnDto>();
            CreateMap<PhotoForCreationDTO, Photo>();           
        }
    }
}