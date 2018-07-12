using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Infrastructure;
using Test.Service.Dto;
using Test.Service.QueryModel;

namespace Test.Service.Interface
{
    public interface ICommentSvc: IDependency
    {
        ResultDto AddSingle(CommentDto dto);

        ResultDto Edit(CommentDto dto);

        Task<ResultDto<CommentDto>> GetPageDataAsync(CommentQueryModel qModel);
    }
}
