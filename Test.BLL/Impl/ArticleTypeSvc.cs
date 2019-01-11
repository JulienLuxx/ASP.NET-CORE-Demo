using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Test.Core.Dto;
using Test.Domain;
using Test.Domain.Entity;
using Test.Domain.Extend;
using Test.Service.Dto;
using Test.Service.Interface;
using Test.Service.QueryModel;

namespace Test.Service.Impl
{
    public class ArticleTypeSvc : BaseSvc, IArticleTypeSvc
    {
        public ArticleTypeSvc(IMapper mapper, TestDBContext testDB) : base(mapper, testDB)
        {
        }

        public ResultDto AddSingle(ArticleTypeDto dto)
        {
            var result = new ResultDto();
            dto.CreateTime = DateTime.Now;
            try
            {
                var data = _mapper.Map<ArticleType>(dto);
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
                var idArray = idString.Split(",");
                var dataList = _testDB.ArticleType.Where(x => x.IsDeleted == false && idArray.Contains(x.Id.ToString()));
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

        public ResultDto Edit(ArticleTypeDto dto)
        {
            var result = new ResultDto();
            dto.CreateTime = DateTime.Now;
            try
            {
                var data = _testDB.ArticleType.Where(x => x.IsDeleted == false && x.Id == dto.Id).FirstOrDefault();
                if (null == data)
                {
                    return result;
                }
                dto.IsDeleted = data.IsDeleted;
                data = _mapper.Map(dto, data);
                _testDB.Update(data);
                var flag = _testDB.SaveChanges();
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

        public async Task<ResultDto> EditAsync(ArticleTypeDto dto)
        {
            var result = new ResultDto();
            try
            {
                dto.CreateTime = DateTime.Now;
                var data = _testDB.ArticleType.Where(x => x.IsDeleted == false && x.Id == dto.Id).FirstOrDefault();
                if (null == data)
                {
                    return result;
                }
                dto.IsDeleted = data.IsDeleted;
                data = _mapper.Map(dto, data);
                _testDB.Update(data);
                await DbContextExtend.MSDNCommitAsync<TestDBContext,ArticleType>(_testDB)
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultDto<ArticleTypeDto>> GetPageDataAsync(ArticleTypeQueryModel qModel)
        {
            var result = new ResultDto<ArticleTypeDto>();
            var query = _testDB.ArticleType.AsNoTracking().Where(x => !x.IsDeleted);
            var queryData = query.Select(x => new ArticleTypeDto()
            {
                Id = x.Id,
                Name = x.Name,
                CreateTime = x.CreateTime,                
            });
            queryData = queryData.OrderBy(o => o.CreateTime);
            queryData = queryData.Skip((qModel.Page - 1) * qModel.PageSize).Take(qModel.PageSize);
            result.ActionResult = true;
            result.Message = "Success";
            result.List = await queryData.ToListAsync();
            return result;
        }
    }
}
