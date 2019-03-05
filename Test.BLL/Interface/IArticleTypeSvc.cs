using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Dto;
using Test.Core.IOC;
using Test.Service.Dto;
using Test.Service.QueryModel;

namespace Test.Service.Interface
{
    public interface IArticleTypeSvc : IDependency 
    {
        ResultDto AddSingle(ArticleTypeDto dto);

        Task<ResultDto> AddSingleAsync(ArticleTypeDto dto);

        ResultDto Delete(string idString);

        Task<ResultDto> DeleteAsync(string idString);

        ResultDto Edit(ArticleTypeDto dto);

        Task<ResultDto> EditAsync(ArticleTypeDto dto);

        Task<ResultDto<ArticleTypeDto>> GetPageDataAsync(ArticleTypeQueryModel qModel);

        ResultDto<ArticleTypeDto> GetSingleData(int id);

        Task<ResultDto<ArticleTypeDto>> GetSingleDataAsync(int id);
    }
}
