using AutoMapper;
using P225Repository.Data.Entities;
using P225Repository.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P225Repository.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryPostDto, Category>()
                .ForMember(des=>des.CreatedAt,src=>src.MapFrom(s=>DateTime.UtcNow.AddHours(4)));

            CreateMap<Category, CategoryListDto>();
            CreateMap<Category, CategoryParentDto>();
            CreateMap<Category, CategoryChildrenDto>();
            CreateMap<Category, CategoryGetDto>();
        }
    }
}
