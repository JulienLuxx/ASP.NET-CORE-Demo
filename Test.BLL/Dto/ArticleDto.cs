using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Core.Dto;

namespace Test.Service.Dto
{
    public class ArticleDto:BaseDto
    {

        public string Title { get; set; }

        public string Content { get; set; }

        public int Type { get; set; }

        public int State { get; set; }

        public int UserId { get; set; }
    }

    public class ArticleDetailDto : ArticleDto
    {
        public ArticleDetailDto()
        {
            Comments = new List<CommentDto>();
            CommentTrees = new List<CommentTreeDto>();
        }

        //[IgnoreMap]
        public List<CommentDto> Comments { get; set; }

        [IgnoreMap]
        public List<CommentTreeDto> CommentTrees { get; set; }
    }
}
