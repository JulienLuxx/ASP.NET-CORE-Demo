using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Core.Dto
{
    public class BaseDto
    {
        public BaseDto()
        {
            IsDeleted = false;
        }
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
