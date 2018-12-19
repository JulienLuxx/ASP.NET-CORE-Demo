using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ResultDto AddSingle(ArticleDto dto)
        {
            var res = new ResultDto();
            dto.CreateTime = DateTime.Now;
            try
            {
                var data = _mapper.Map<Article>(dto);
                _testDB.Add(data);
                var flag = _testDB.SaveChanges();
                if (flag > 0)
                {
                    res.ActionResult = true;
                    res.Message = "Success";
                }
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<ResultDto> AddSingleAsync(ArticleDto dto)
        {
            var res = new ResultDto();
            var data = _mapper.Map<Article>(dto);
            await _testDB.AddAsync(data);
            var flag = await _testDB.SaveChangesAsync();
            if (flag > 0)
            {
                res.ActionResult = true;
                res.Message = "Success";
            }
            return res;
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
            var res = new ResultDto<ArticleDto>();
            var query = _testDB.Article.AsNoTracking().Where(x=>x.IsDeleted==false);
            query = qModel.Status.HasValue ? query.Where(x => x.Status == qModel.Status) : query;
            query = qModel.UserId.HasValue ? query.Where(x => x.UserId == qModel.UserId) : query;
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
            res.ActionResult = true;
            res.Message = "Success";
            res.List = queryData.ToList();
            return res;
        }

        public async Task<ResultDto<ArticleDto>> GetPageDataAsync(ArticleQueryModel qModel)
        {
            var res = new ResultDto<ArticleDto>();
            var query = _testDB.Article.AsNoTracking().Where(x => x.IsDeleted == false);
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
            res.ActionResult = true;
            res.Message = "Success";
            res.List = await queryData.ToListAsync();
            return res;
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
            var res = new ResultDto<ArticleDetailDto>();
            var data = await _testDB.Article.AsNoTracking().Where(x => x.Id == Id&&x.IsDeleted==false).Include(x => x.Comments).FirstOrDefaultAsync();
            if (null != data)
            {
                var dto = _mapper.Map<ArticleDetailDto>(data);
                dto.CommentTrees = GetAllCommentByTree(dto.Comments);
                res.ActionResult = true;
                res.Message = "Success";
                res.Data = dto;
            }
            return res;
        }

        public List<CommentTreeDto> GetAllCommentByTree(List<CommentDto> dtoList)
        {
            var treeList = new List<CommentTreeDto>();
            var rootList = dtoList.Where(x => x.ParentId == 0);
            foreach (var item in rootList)
            {
                var tree = new CommentTreeDto();
                //GetTree(item, tree, dtoList);
                _util.GetDtoTree(item, tree, dtoList);
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
