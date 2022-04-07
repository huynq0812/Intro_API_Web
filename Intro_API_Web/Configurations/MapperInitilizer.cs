using AutoMapper;
using Intro_API_Web.Controllers.Data;
using Intro_API_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intro_API_Web.Configurations
{
    public class MapperInitilizer : Profile
    {
        public MapperInitilizer()
        {
            CreateMap<ApiUser, UserDTO>().ReverseMap();
        }
    }
}
