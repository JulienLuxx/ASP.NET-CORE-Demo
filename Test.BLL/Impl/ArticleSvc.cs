using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Dto;
using Test.Core.Tree;
using Test.Domain;
using Test.Domain.Entity;
using Test.Service.Dto;
using Test.Service.Interface;
using Test.Service.QueryModel;

namespace Test.Service.Impl
{
    public class ArticleSvc: BaseSvc,IArticleSvc
    {
        private ITreeUtil _util { get; set; }

        private ICommentSvc _commentSvc { get; set; }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="testDB"></param>
        /// <param name="util"></param>
        /// <param name="commentSvc"></param>
        public ArticleSvc(
            IMapper mapper,
            TestDBContext testDB,
            ITreeUtil util,
            ICommentSvc commentSvc
            ) :base(mapper,testDB)
        {
            _util = util;
            _commentSvc = commentSvc;
        }

        public ResultDto AddSingle(string dataJson)
        {
            var result = new ResultDto();
            if (!string.IsNullOrEmpty(dataJson) && !string.IsNullOrWhiteSpace(dataJson))
            {
                var dto = JsonConvert.DeserializeObject<ArticleDto>(dataJson);
                result = AddSingle(dto);
            }
            return result;
        }

        public ResultDto AddSingle(ArticleDto dto)
        {
            var result = new ResultDto();
            dto.SetDefaultValue();
            if (0 == dto.TypeId)
            {
                dto.TypeId = 1;
            }
            try
            {
                var data = _mapper.Map<Article>(dto);
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

        public async Task<ResultDto> AddSingleAsync(ArticleDto dto)
        {
            var result = new ResultDto();
            var data = _mapper.Map<Article>(dto);
            await _testDB.AddAsync(data);
            var flag = await _testDB.SaveChangesAsync();
            if (flag > 0)
            {
                result.ActionResult = true;
                result.Message = "Success";
            }
            return result;
        }

        public ResultDto Delete(string idString)
        {
            var result = new ResultDto();
            try
            {
                var idArray = idString.Split(",");
                var dataList = _testDB.Article.Where(x => x.IsDeleted == false && idArray.Contains(x.Id.ToString()));
                foreach (var data in dataList)
                {
                    data.IsDeleted = true;
                }
                var flag = _testDB.SaveChanges();
                if (0 < flag)
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

        public ResultDto Edit(string dataJson)
        {
            var result = new ResultDto();
            if (!string.IsNullOrEmpty(dataJson) && !string.IsNullOrWhiteSpace(dataJson))
            {
                var dto = JsonConvert.DeserializeObject<ArticleDto>(dataJson);
                result = Edit(dto);
            }
            return result;
        }

        public ResultDto Edit(ArticleDto dto)
        {
            var res = new ResultDto();
            dto.CreateTime = DateTime.Now;
            try
            {
                var data = _testDB.Article.Where(x => x.IsDeleted == false && x.Id == dto.Id).FirstOrDefault();
                if (null == data)
                {
                    return res;
                }
                dto.IsDeleted = data.IsDeleted;
                dto.UserId = data.UserId;
                dto.TypeId = data.TypeId;
                dto.Status = data.Status;
                data = _mapper.Map(dto, data);
                _testDB.Update(data);
                var flag= _testDB.SaveChanges();
                if (0 < flag)
                {
                    res.ActionResult = true;
                    res.Message = "success";
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return res;
        }

        public ResultDto<ArticleDto> GetPageData(ArticleQueryModel qModel)
        {
            var result = new ResultDto<ArticleDto>();
            var query = _testDB.Article.AsNoTracking().Include(x=>x.ArticleType).Where(x=>x.IsDeleted==false);
            query = qModel.Status.HasValue ? query.Where(x => x.Status == qModel.Status) : query;
            query = string.IsNullOrEmpty(qModel.TypeName) ? query.Where(x => x.ArticleType.Name.Contains(qModel.TypeName)) : query;
            var queryData = query.Select(x => new ArticleDto()
            {
                Id=x.Id,
                Title=x.Title,
                Content=x.Content,
                TypeId=x.TypeId,
                CreateTime=x.CreateTime
            });
            queryData = queryData.OrderBy(o => o.CreateTime);
            queryData = queryData.Skip((qModel.Page - 1) * qModel.PageSize).Take(qModel.PageSize);
            result.ActionResult = true;
            result.Message = "Success";
            result.List = queryData.ToList();
            return result;
        }

        public async Task<ResultDto<ArticleDto>> GetPageDataAsync(ArticleQueryModel qModel)
        {
            var result = new ResultDto<ArticleDto>();
            var query = _testDB.Article.AsNoTracking().Include(x => x.ArticleType).Where(x => x.IsDeleted == false);
            query = qModel.Status.HasValue ? query.Where(x => x.Status == qModel.Status) : query;
            query = qModel.UserId.HasValue ? query.Where(x => x.UserId == qModel.UserId) : query;
            query = string.IsNullOrEmpty(qModel.TypeName) ? query.Where(x => x.ArticleType.Name.Contains(qModel.TypeName)) : query;
            var queryData = query.Select(x => new ArticleDto()
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                TypeId = x.TypeId,
                CreateTime = x.CreateTime
            });
            queryData = queryData.OrderBy(o => o.CreateTime);
            queryData = queryData.Skip((qModel.Page - 1) * qModel.PageSize).Take(qModel.PageSize);
            result.ActionResult = true;
            result.Message = "Success";
            result.List = await queryData.ToListAsync();
            return result;
        }

        public ResultDto<ArticleDetailDto> GetSingleData(int Id)
        {
            var res = new ResultDto<ArticleDetailDto>();
            var data = _testDB.Article.AsNoTracking().Where(x => x.Id == Id&&x.IsDeleted==false).Include(x => x.Comments).FirstOrDefault();
            if (null != data)
            {
                var dto = _mapper.Map<ArticleDetailDto>(data);
                res.ActionResult = true;
                res.Message = "Success";
                res.Data = dto;                
            }
            return res;
        }

        public async Task<ResultDto<ArticleDetailDto>> GetSingleDataAsync(int Id)
        {
            var result = new ResultDto<ArticleDetailDto>();
            var data = await _testDB.Article.AsNoTracking().Where(x => x.Id == Id&&x.IsDeleted==false).Include(x => x.Comments).FirstOrDefaultAsync();
            if (null != data)
            {
                var dto = _mapper.Map<ArticleDetailDto>(data);
                //dto.CommentTrees = GetAllCommentByTree(dto.Comments);
                dto.CommentTrees = _commentSvc.GetCommentTrees(dto.Comments);
                result.ActionResult = true;
                result.Message = "Success";
                result.Data = dto;
            }
            return result;
        }

        public List<CommentTreeDto> GetAllCommentByTree(List<CommentDto> dtoList)
        {
            var treeList = new List<CommentTreeDto>();
            var rootList = dtoList.Where(x => x.ParentId == 0);
            foreach (var item in rootList)
            {
                var tree = new CommentTreeDto();
                GetTree(item, tree, dtoList);
                treeList.Add(tree);
            }
            return treeList;
        }
               
        private void GetTree(CommentDto dto, CommentTreeDto tree, List<CommentDto> list)
        {
            if (null == dto)
            {
                return;
            }
            tree = Mapper.Map(dto,tree);
            var childs = list.Where(x => x.ParentId == dto.Id).ToList();
            foreach (var child in childs)
            {
                var node = new CommentTreeDto();
                tree.Childrens.Add(node);
                GetTree(child, node, list);
            }
        }

    }
}
