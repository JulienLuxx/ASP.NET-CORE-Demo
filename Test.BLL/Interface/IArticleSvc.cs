﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Infrastructure;
using Test.Service.Dto;
using Test.Service.QueryModel;

namespace Test.Service.Interface
{
    public interface IArticleSvc: IDependency
    {
        ResultDto AddSingle(ArticleDto dto);

        Task<ResultDto> AddSingleAsync(ArticleDto dto);

        Task<ResultDto<ArticleDto>> GetPageDataAsync(ArticleQueryModel qModel);

        Task<ResultDto<ArticleDetailDto>> GetSingleDataAsync(int Id);
    }
}
