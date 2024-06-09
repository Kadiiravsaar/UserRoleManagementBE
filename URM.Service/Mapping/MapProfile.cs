using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URM.Core.DTOs;
using URM.Core.Models;

namespace URM.Service.Mapping
{
	public class MapProfile : Profile
	{
		public MapProfile()
		{
			CreateMap<AppUser, UserDto>();
			CreateMap<AppUser, UserDetailDto>()
			.ForMember(dest => dest.Roles, opt => opt.Ignore());  // Rolleri manuel ekleyeceğimiz için atlıyoruz
			
			// Role for mapping
			CreateMap<AppRole, RoleDto>().ReverseMap();

			CreateMap<RegisterDto, AppUser>()
		   .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
		   .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

			CreateMap<AppUser, NewUserDto>()
				.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));
		}
	}
}
