using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.Core.Dto;

namespace Test.Core.Tree
{
    /// <summary>
    /// TreeUtil
    /// </summary>
    public class TreeUtil: ITreeUtil
    {
        /// <summary>
        /// GetTreeForBaseDto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Ttree"></typeparam>
        /// <param name="dto"></param>
        /// <param name="tree"></param>
        /// <param name="list"></param>
        public void GetDtoTree<T, Ttree>(T dto, BaseTreeDto<Ttree> tree, List<T> list) where T : BaseDto, ITreeDto, new() where Ttree : BaseTreeDto<Ttree>, new()
        {
            try
            {
                if (null == dto)
                {
                    return;
                }
                tree = Mapper.Map(dto, tree);
                Func<T, bool> func = f => f.ParentId == dto.Id;
                var childs = list.Where(func).ToList();
                foreach (var child in childs)
                {
                    Ttree node = new Ttree();
                    tree.Childrens.Add(node);
                    GetDtoTree(child, node, list);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
