using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test.Core.Dto;

namespace Test.Core.Tree
{
    public class TreeUtil: ITreeUtil
    {
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
            { }
        }

        public void GetDataTree<T, Ttree>(T data, BaseTreeDto<Ttree> tree, List<T> list) where T : BaseDto, ITreeDto, new() where Ttree : BaseTreeDto<Ttree>, new()
        {
            try
            {
                if (null == data)
                {
                    return;
                }
                tree = Mapper.Map(data, tree);
                Func<T, bool> func = f => f.ParentId == data.Id;
                var childs = list.Where(func).ToList();
                foreach (var child in childs)
                {
                    Ttree node = new Ttree();
                    tree.Childrens.Add(node);
                    GetDataTree(child, node, list);
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
