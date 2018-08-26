using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Core.Dto
{
    public interface ITreeDto
    {
        int ParentId { get; set; }
    }
    public class BaseTreeDto<T>
    {
        public BaseTreeDto()
        {
            Childrens = new List<T>();
        }
        [IgnoreMap]
        public List<T> Childrens { get; set; }
    }
}
