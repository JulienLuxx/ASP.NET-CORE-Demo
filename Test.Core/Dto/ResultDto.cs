using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Core.Dto
{
    public partial class ResultDto<T> where T : class
    {
        public ResultDto()
        {
            ActionResult = false;
            List = new List<T>();
        }
        public bool ActionResult { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public List<T> List { get; set; }
    }

    public partial class ResultDto
    {
        public ResultDto()
        {
            ActionResult = false;
        }
        public bool ActionResult { get; set; }

        public string Message { get; set; }
    }

    public class ResultBaseDto
    {
        public int ActionResult { get; set; }

        public string Message { get; set; }

        public bool HaveData { get; set; }

        public dynamic Data { get; set; }
    }
}
