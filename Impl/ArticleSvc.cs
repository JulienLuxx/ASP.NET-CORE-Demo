﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Entity;
using Test.Service.Dto;
using Test.Service.Interface;
using Test.Service.QueryModel;

namespace Test.Service.Impl
{
    public class ArticleSvc: BaseSvc,IArticleSvc
    {
        public ArticleSvc(IMapper mapper) :base(mapper)
        {
        }
        public ResultDto AddSingle(ArticleDto dto)
        {
            var res = new ResultDto();
            dto.CreateTime = DateTime.Now;
            var data = _mapper.Map<Article>(dto);
            TestDB.Add(data);
            var flag = TestDB.SaveChanges();
            if (flag > 0)
            {
                res.ActionResult = true;
                res.Msg = "Success";
            }
            return res;
        }

        public async Task<ResultDto> AddSingleAsync(ArticleDto dto)
        {
            var res = new ResultDto();
            var data = _mapper.Map<Article>(dto);
            await TestDB.AddAsync(data);
            var flag = await TestDB.SaveChangesAsync();
            if (flag > 0)
            {
                res.ActionResult = true;
                res.Msg = "Success";
            }
            return res;
        }

        public ResultDto<ArticleDto> GetPageData(ArticleQueryModel qModel)
        {
            var res = new ResultDto<ArticleDto>();
            var query = TestDB.Article.AsNoTracking().Where(x=>x.IsDeleted==false);
            query = qModel.State.HasValue ? query.Where(x => x.State == qModel.State) : query;
            var queryData = query.Select(x => new ArticleDto()
            {
                Id=x.Id,
                Title=x.Title,
                Content=x.Content,
                Type=x.Type,
                CreateTime=x.CreateTime
            });
            queryData = queryData.OrderBy(o => o.CreateTime);
            queryData = queryData.Skip((qModel.Page - 1) * qModel.PageSize).Take(qModel.PageSize);
            res.ActionResult = true;
            res.Msg = "Success";
            res.List = queryData.ToList();
            return res;
        }

        public async Task<ResultDto<ArticleDto>> GetPageDataAsync(ArticleQueryModel qModel)
        {
            var res = new ResultDto<ArticleDto>();
            var query = TestDB.Article.AsNoTracking();
            var queryData = query.Select(x => new ArticleDto()
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                Type = x.Type,
                CreateTime = x.CreateTime
            });
            queryData = queryData.OrderBy(o => o.CreateTime);
            queryData = queryData.Skip((qModel.Page - 1) * qModel.PageSize).Take(qModel.PageSize);
            res.ActionResult = true;
            res.Msg = "Success";
            res.List = await queryData.ToListAsync();
            return res;
        }

        public ResultDto<ArticleDetailDto> GetSingleData(int Id)
        {
            var res = new ResultDto<ArticleDetailDto>();
            var data = TestDB.Article.AsNoTracking().Where(x => x.Id == Id&&x.IsDeleted==false).Include(x => x.Comments).FirstOrDefault();
            if (null != data)
            {
                var dto = _mapper.Map<ArticleDetailDto>(data);
                res.ActionResult = true;
                res.Msg = "Success";
                res.Data = dto;                
            }
            return res;
        }

        public async Task<ResultDto<ArticleDetailDto>> GetSingleDataAsync(int Id)
        {
            var res = new ResultDto<ArticleDetailDto>();
            var data = await TestDB.Article.AsNoTracking().Where(x => x.Id == Id&&x.IsDeleted==false)/*.Include(x => x.Comments)*/.FirstOrDefaultAsync();
            if (null != data)
            {
                var dto = _mapper.Map<ArticleDetailDto>(data);
                dto.CommentTrees = GetAllCommentByTree(dto.Comments);
                res.ActionResult = true;
                res.Msg = "Success";
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
