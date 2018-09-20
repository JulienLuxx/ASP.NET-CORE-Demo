﻿using System;
using System.Collections.Generic;
using System.Text;
using Test.Core.Dto;

namespace Test.Core.Tree
{
    /// <summary>
    /// TreeUtil
    /// </summary>
    public interface ITreeUtil
    {
        /// <summary>
        /// Util.GetTreeFromBaseDtoList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Ttree"></typeparam>
        /// <param name="dto"></param>
        /// <param name="tree"></param>
        /// <param name="list"></param>
        void GetTree<T, Ttree>(T dto, BaseTreeDto<Ttree> tree, List<T> list) where T : BaseDto, ITreeDto, new() where Ttree : BaseTreeDto<Ttree>, new();
    }
}