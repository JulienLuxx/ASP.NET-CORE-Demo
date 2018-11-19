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

        ResultDto Delete(string ids);

        ResultDto Edit(CommentDto dto);

        List<CommentTreeDto> GetCommentTrees(List<CommentDto> dtoList, int parentId = 0);

        Task<ResultDto<CommentTreeDto>> GetSingleDataAsync(int id);

        Task<ResultDto<CommentDto>> GetPageDataAsync(CommentQueryModel qModel);
    }
}
