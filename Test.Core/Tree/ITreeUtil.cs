using System;
using System.Collections.Generic;
using System.Text;
using Test.Core.Dto;

namespace Test.Core.Tree
{
    public interface ITreeUtil
    {
        void GetTree<T, Ttree>(T dto, BaseTreeDto<Ttree> tree, List<T> list) where T : BaseDto, ITreeDto, new() where Ttree : BaseTreeDto<Ttree>, new();
    }
}
