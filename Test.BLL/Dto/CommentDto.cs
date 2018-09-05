using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Core.Dto;

namespace Test.Service.Dto
{
    public class CommentDto : BaseDto, ITreeDto
    {
        public CommentDto()
        {
        }
        public string Creator { get; set; }

        public string Content { get; set; }

        public int ParentId { get; set; }

        public int State { get; set; }

        public int? ArticleId { get; set; }

        //public ArticleDto Article { get; set; }
    }

    public class CommentTreeDto : BaseTreeDto<CommentTreeDto>
    {
        public int Id { get; set; }

        public string Creator { get; set; }

        public string Content { get; set; }

        public int ParentId { get; set; }

        public int State { get; set; }

        public int? ArticleId { get; set; }
    }
}
