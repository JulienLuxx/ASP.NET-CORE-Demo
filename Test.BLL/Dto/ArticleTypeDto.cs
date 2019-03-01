using System;
using System.Collections.Generic;
using System.Text;
using Test.Core.Dto;

namespace Test.Service.Dto
{
    public class ArticleTypeDto:BaseDto
    {
        public string Name { get; set; }

        public string EditerName { get; set; }

        public int Status { get; set; }
    }
}
