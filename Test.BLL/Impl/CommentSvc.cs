using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Test.Domain.Entity;
using Test.Service.Dto;
using Test.Service.Interface;
using Test.Service.QueryModel;

namespace Test.Service.Impl
{
    public class CommentSvc : BaseSvc,ICommentSvc
    {
        public CommentSvc(IMapper mapper) : base(mapper)
        {
        }

        public ResultDto AddSingle(CommentDto dto)
        {
            var res = new ResultDto();
            dto.CreateTime = DateTime.Now;
            var data = _mapper.Map<Comment>(dto);
            TestDB.Add(data);
            var flag = TestDB.SaveChanges();
            if (flag > 0)
            {
                res.ActionResult = true;
                res.Msg = "Success";
            }
            return res;
        }

        public async Task<ResultDto<CommentDto>> GetPageDataAsync(CommentQueryModel qModel)
        {
            var res = new ResultDto<CommentDto>();
            var query=TestDB.Comment.AsNoTracking();
            var queryData = query.Select(x => new CommentDto()
            {
                Id = x.Id,
                ArticleId=x.ArticleId,
                Content = x.Content,
                State = x.State,
                CreateTime = x.CreateTime
            });
            queryData = queryData.OrderBy(o => o.CreateTime);
            queryData = queryData.Skip((qModel.Page - 1) * qModel.PageSize).Take(qModel.PageSize);
            res.ActionResult = true;
            res.Msg = "Success";
            res.List = await queryData.ToListAsync();
            return res;
        }
    }
}
