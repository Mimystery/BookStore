using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Core.Models;
using BookStrore.DataAccess.Entities;

namespace BookStore.Application.Mappings
{
    public class UserMappingProfile : Profile    
    {
        public UserMappingProfile()
        {
            CreateMap<UserEntity, User>().ReverseMap();
        }
    }
}
