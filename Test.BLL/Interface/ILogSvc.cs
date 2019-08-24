using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Service.QueryModel;

namespace Test.Service.Interface
{
    public interface ILogSvc
    {
        Task<dynamic> GetPageDataAsync(LogQueryModel qModel);
    }
}
