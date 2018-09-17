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

            CreateMap<Comment, CommentDto>();
            CreateMap<Domain.LazyLoadEntity.Comment, CommentDto>();
            CreateMap<CommentDto, Comment>();
            CreateMap<CommentDto, CommentTreeDto>()/*.ForMember(x => x.Childrens, a => a.Ignore())*/;

            CreateMap<UserDto, User>();
            CreateMap<RegisterDto, UserDto>();

        }
    }
}
