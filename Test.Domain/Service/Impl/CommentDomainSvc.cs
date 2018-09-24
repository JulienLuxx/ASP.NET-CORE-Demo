using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Test.Domain.Service.Interface;

namespace Test.Domain.Service.Impl
{
    public class CommentDomainSvc : BaseDomainSvc, ICommentDomainSvc
    {
        private IMapper _mapper { get; set; }
        private TestDBContext _testDB { get; set; }
        public CommentDomainSvc(IMapper mapper, TestDBContext testDB) : base(mapper, testDB)
        {
        }
    }
}
