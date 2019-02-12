using System;
using System.Collections.Generic;
using System.Text;
using Test.Core.Dto;
using Test.Core.IOC;

namespace Test.Core.Tree
{
    /// <summary>
    /// TreeUtil
    /// </summary>
    public interface ITreeUtil : IDependency 
    {
        /// <summary>
        /// Util.GetTreeFromBaseDtoList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Ttree"></typeparam>
        /// <param name="dto"></param>
        /// <param name="tree"></param>
        /// <param name="list"></param>
        void GetDtoTree<T, Ttree>(T dto, BaseTreeDto<Ttree> tree, List<T> list) where T : BaseDto, ITreeDto, new() where Ttree : BaseTreeDto<Ttree>, new();
    }
}
