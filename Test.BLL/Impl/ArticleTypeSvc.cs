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
        private IMapper _mapper { get; set; }
        private IDbContextExtendSvc _dbContextExtendSvc { get; set; }
        public ArticleTypeSvc(
            TestDBContext testDB,
            IDbContextExtendSvc dbContextExtendSvc
            ) : base( testDB)
        {
            _dbContextExtendSvc = dbContextExtendSvc;
        }

        public ArticleTypeSvc(
            IMapper mapper,
            TestDBContext testDB,
            IDbContextExtendSvc dbContextExtendSvc
            ) : base(testDB)
        {
            _mapper = mapper;
            _dbContextExtendSvc = dbContextExtendSvc;
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

        public async Task<ResultDto> AddSingleAsync(ArticleTypeDto dto)
        {
            var result = new ResultDto();
            dto.CreateTime = DateTime.Now;
            try
            {
                var data = Mapper.Map<ArticleType>(dto);
                _testDB.ArticleType.Add(data);
                var flag = await _testDB.SaveChangesAsync();
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

        public ArticleType Add(ArticleTypeDto dto)
        {
            var data = new ArticleType();
            dto.CreateTime = DateTime.Now;
            data = Mapper.Map<ArticleType>(dto);
            _testDB.Add(data);
            var flag = _testDB.SaveChanges();
            return data;
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

        public async Task<ResultDto> DeleteAsync(string idString)
        {
            var result = new ResultDto();
            try
            {
                var idArray = idString.Split(",");
                var dataList = await _testDB.ArticleType.Where(x => x.IsDeleted == false && idArray.Contains(x.Id.ToString())).ToListAsync();
                foreach (var data in  dataList)
                {
                    data.IsDeleted = true;
                }
                _testDB.UpdateRange(dataList);
                var flag = await _testDB.SaveChangesAsync();
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
                data = Mapper.Map(dto, data);
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
            dto.CreateTime = DateTime.Now;
            try
            {
                var data = await _testDB.ArticleType.Where(x => x.IsDeleted == false && x.Id == dto.Id).FirstOrDefaultAsync();
                if (null == data)
                {
                    return result;
                }
                dto.IsDeleted = data.IsDeleted;
                data = Mapper.Map(dto, data);
                //_testDB.Update(data);
                //await _testDB.Database.ExecuteSqlCommandAsync("")
                var flag = await _dbContextExtendSvc.CommitTestAsync<TestDBContext, ArticleType>(_testDB, true);
                if (0 < flag)
                {
                    result.ActionResult = true;
                    result.Message = "success";
                }
                else if (flag == -1)
                {
                    result.Message = "Data has been change , Please try again!";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public ResultDto<ArticleTypeDto> GetPageData(ArticleTypeQueryModel qModel)
        {
            var result = new ResultDto<ArticleTypeDto>();
            var query = _testDB.ArticleType.AsNoTracking().Where(x => !x.IsDeleted);
            var queryData = query.Select(x => new ArticleTypeDto()
            {
                Id = x.Id,
                Name = x.Name,
                EditerName=x.EditerName,
                CreateTime = x.CreateTime,
            });
            queryData = queryData.OrderBy(o => o.CreateTime);
            queryData = queryData.Skip((qModel.PageIndex - 1) * qModel.PageSize).Take(qModel.PageSize);
            result.ActionResult = true;
            result.Message = "Success";
            result.List = queryData.ToList();
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
                EditerName = x.EditerName,
                CreateTime = x.CreateTime,
            });
            queryData = queryData.OrderBy(o => o.CreateTime);
            queryData = queryData.Skip((qModel.PageIndex - 1) * qModel.PageSize).Take(qModel.PageSize);
            result.ActionResult = true;
            result.Message = "Success";
            result.List = await queryData.ToListAsync();
            return result;
        }

        public ResultDto<ArticleTypeDto> GetSingleData(int id)
        {
            var result = new ResultDto<ArticleTypeDto>();
            var data = _testDB.ArticleType.AsNoTracking().Where(x => !x.IsDeleted && x.Id == id).FirstOrDefault();
            if (null != data)
            {
                var dto = Mapper.Map<ArticleTypeDto>(data);
                result.Data = dto;
                result.ActionResult = true;
                result.Message = "success";
            }
            return result;
        }

        public async Task<ResultDto<ArticleTypeDto>> GetSingleDataAsync(int id)
        {
            var result = new ResultDto<ArticleTypeDto>();
            var data =await  _testDB.ArticleType.AsNoTracking().Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (null != data)
            {
                var dto = Mapper.Map<ArticleTypeDto>(data);
                result.Data = dto;
                result.ActionResult = true;
                result.Message = "success";
            }
            return result;
        }
    }
}
