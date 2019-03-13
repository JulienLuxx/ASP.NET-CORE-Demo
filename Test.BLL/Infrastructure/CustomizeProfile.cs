using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Domain.Entity;
using Test.Service.Dto;

namespace Test.Service.Infrastructure
{
    public class CustomizeProfile:Profile
    {
        public CustomizeProfile()
        {
            CreateMap<Article, ArticleDto>();
            CreateMap<ArticleDto, Article>();
            CreateMap<Article, ArticleDetailDto>();
            CreateMap<Domain.LazyLoadEntity.Article, ArticleDetailDto>();

            CreateMap<ArticleType, ArticleTypeDto>();
            CreateMap<ArticleTypeDto, ArticleType>();

            CreateMap<Comment, CommentDto>();
            CreateMap<Domain.LazyLoadEntity.Comment, CommentDto>();
            CreateMap<CommentDto, Comment>();
            CreateMap<Comment, CommentTreeDto>();
            CreateMap<CommentDto, CommentTreeDto>()/*.ForMember(x => x.Childrens, a => a.Ignore())*/;

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<UserDto, RegisterDto>();
            CreateMap<RegisterDto, UserDto>()/*.ForMember(x => x.CreateTime, a => a.Ignore()).ForMember(x => x.Status, a => a.Ignore())*/;


        }
    }
}
