using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Test.Core.Dto;
using Test.Core.Tree;
using Test.Domain;
using Test.Domain.Entity;
using Test.Domain.Extend;
using Test.Service.Dto;
using Test.Service.Interface;
using Test.Service.QueryModel;

namespace Test.Service.Impl
{
    public class CommentSvc : BaseSvc,ICommentSvc
    {
        private ITreeUtil _util { get; set; }

        private IDbContextExtendSvc _dbContextExtendSvc { get; set; }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="testDB"></param>
        /// <param name="dbContextExtendSvc"></param>
        /// <param name="util"></param>
        public CommentSvc(
            TestDBContext testDB,
            IDbContextExtendSvc dbContextExtendSvc,
            ITreeUtil util
            ) : base(testDB)
        {
            _dbContextExtendSvc = dbContextExtendSvc;
            _util = util;
        }

        public ResultDto AddSingle(CommentDto dto)
        {
            var result = new ResultDto();
            dto.CreateTime = DateTime.Now;
            try
            {
                var data = Mapper.Map<Comment>(dto);
                _testDB.Add(data);
                var flag = _testDB.SaveChanges();
                if (flag > 0)
                {
                    result.ActionResult = true;
                    result.Message = "Success";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public ResultDto Delete(string idString)
        {
            var result = new ResultDto();
            try
            {
                var idArray = idString.Split(',');
                var dataList = _testDB.Comment.Where(x => idArray.Contains(x.Id.ToString())).ToList();
                foreach (var item in dataList)
                {
                    item.IsDelete = true;
                }
                var flag = _testDB.SaveChanges();
                if (0 < flag)
                {
                    result.ActionResult = true;
                    result.Message = "Sucess";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public ResultDto Edit(CommentDto dto)
        {
            var result = new ResultDto();
            dto.CreateTime = DateTime.Now;
            try
            {
                var data = _testDB.Comment.Where(x => x.IsDelete == false && x.Id == dto.Id).FirstOrDefault();
                if (null == data)
                {
                    return result;
                }
                dto.IsDeleted = data.IsDelete;
                data = _mapper.Map(dto, data);
                _testDB.Update(data);
                var flag= _testDB.SaveChanges();
                if (0 < flag)
                {
                    result.ActionResult = true;
                    result.Message = "success";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public List<CommentTreeDto> GetCommentTrees(List<CommentDto> dtoList, int parentId = 0)
        {
            var treeList = new List<CommentTreeDto>();
            var rootList = dtoList.Where(x => x.ParentId == parentId);
            foreach (var item in rootList)
            {
                var tree = new CommentTreeDto();
                _util.GetDtoTree(item, tree, dtoList);
                treeList.Add(tree);
            }
            return treeList;
        }

        public async Task<ResultDto<CommentDto>> GetPageDataAsync(CommentQueryModel qModel)
        {
            var result = new ResultDto<CommentDto>();
            var query = _testDB.Comment.AsNoTracking().Where(x => !x.IsDelete);
            query = qModel.State.HasValue ? query.Where(x => x.Status == qModel.State) : query;
            var queryData = query.Select(x => new CommentDto()
            {
                Id = x.Id,
                ArticleId = x.ArticleId,
                Content = x.Content,
                Status = x.Status,
                ParentId = x.ParentId ?? 0,
                CreateTime = x.CreateTime
            });
            queryData = queryData.OrderBy(o => o.CreateTime);
            queryData = queryData.Skip((qModel.Page - 1) * qModel.PageSize).Take(qModel.PageSize);
            result.ActionResult = true;
            result.Message = "Success";
            result.List = await queryData.ToListAsync();
            return result;
        }

        public async Task<ResultDto<CommentTreeDto>> GetSingleDataAsync(int id)
        {
            var result = new ResultDto<CommentTreeDto>();
            var treeDtoList = new List<CommentTreeDto>();
            var dataList = await _testDB.Comment.AsNoTracking().Where(x => x.IsDelete == false).ToListAsync();
            if (dataList.Where(x=>x.Id==id).Any()) 
            {
                var dtoList = _mapper.Map<List<CommentDto>>(dataList);
                treeDtoList = GetCommentTrees(dtoList, id);
                var data = dataList.Where(x => x.Id == id).FirstOrDefault();
                if (null != data)
                {
                    var treeDto = _mapper.Map<CommentTreeDto>(data);
                    treeDtoList.Insert(0, treeDto);
                }
                result.ActionResult = true;
                result.Message = "Success";
                result.List = treeDtoList;
            }
            return result;
        }
    }
}
