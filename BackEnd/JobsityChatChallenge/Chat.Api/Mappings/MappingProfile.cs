using AutoMapper;
using Chat.Core.DTO;
using Chat.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NewUserRequestDto, ApplicationUser>();
        }
    }
}
