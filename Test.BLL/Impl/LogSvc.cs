using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Dto;
using Test.Core.Test;
using Test.Domain;
using Test.Service.Interface;
using Test.Service.QueryModel;

namespace Test.Service.Impl
{
    public class LogSvc : BaseSvc, ILogSvc
    {
        public LogSvc(TestDBContext testDB) : base(testDB)
        {
        }

        public async Task<dynamic> GetPageDataAsync(LogQueryModel qModel)
        {
            var query = _testDB.Log.AsNoTracking();

            query = !string.IsNullOrEmpty(qModel.OrderByColumn) ? query.OrderBy(qModel.OrderByColumn, qModel.IsDesc) : query.OrderByDescending(o => o.Id);
            query = query.Skip((qModel.PageIndex - 1) * qModel.PageSize).Take(qModel.PageSize);
            var result = new ResultDto<dynamic>()
            {
                ActionResult = true,
                Message = "Success",
                Data = await query.ToListAsync()
            };
            return result;
        }
    }
}
