using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Test.Domain;
using Test.Domain.Entity;
using Test.Service.Dto;
using Test.Service.Interface;
using Test.Service.QueryModel;

namespace Test.Service.Impl
{
    public class CommentSvc : BaseSvc,ICommentSvc
    {
        public CommentSvc(IMapper mapper, TestDBContext testDB) : base(mapper,testDB)
        {
        }

        public ResultDto AddSingle(CommentDto dto)
        {
            var res = new ResultDto();
            dto.CreateTime = DateTime.Now;
            try
            {
                var data = _mapper.Map<Comment>(dto);
                _testDB.Add(data);
                var flag = _testDB.SaveChanges();
                if (flag > 0)
                {
                    res.ActionResult = true;
                    res.Msg = "Success";
                }
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }

            return res;
        }

        public ResultDto Delete(string ids)
        {
            var res = new ResultDto();
            try
            {
                var idArray = ids.Split(',');
                var dataList = _testDB.Comment.Where(x => idArray.Contains(x.Id.ToString())).ToList();
                foreach (var item in dataList)
                {
                    item.IsDelete = true;
                }
                _testDB.SaveChanges();
                res.ActionResult = true;
                res.Msg = "Sucess";
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }



        public ResultDto Edit(CommentDto dto)
        {
            var res = new ResultDto();
            dto.CreateTime = DateTime.Now;
            try
            {
                var data = _testDB.Comment.Where(x => x.IsDelete == false && x.Id == dto.Id).FirstOrDefault();
                if (null == data)
                {
                    return res;
                }
                dto.IsDeleted = data.IsDelete;
                data = _mapper.Map(dto, data);
                _testDB.Update(data);
                var flag= _testDB.SaveChanges();
                if (0 < flag)
                {
                    res.ActionResult = true;
                    res.Msg = "success";
                }
            }
            catch (Exception ex)
            {
                res.Msg = ex.Message;
            }
            return res;
        }

        public async Task<ResultDto<CommentDto>> GetPageDataAsync(CommentQueryModel qModel)
        {
            var res = new ResultDto<CommentDto>();
            var query=_testDB.Comment.AsNoTracking();
            query = query.Where(x => qModel.State.HasValue && x.State == qModel.State);
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
