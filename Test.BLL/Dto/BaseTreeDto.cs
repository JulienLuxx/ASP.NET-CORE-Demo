using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Service.Dto
{
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
