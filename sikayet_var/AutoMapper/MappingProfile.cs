using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;

namespace sikayet_var.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Brand, BrandDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Comment, CommentDto>();
            CreateMap<Complaint, ComplaintDto>();
            CreateMap<Like, LikeDto>();
            CreateMap<Notification, NotificationDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<Report, ReportDto>();
            CreateMap<User, UserDto>();

            CreateMap<Brand, BrandDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Complaint, ComplaintDto>().ReverseMap();
            CreateMap<Like, LikeDto>().ReverseMap();
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Report, ReportDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();



        }
    }
}