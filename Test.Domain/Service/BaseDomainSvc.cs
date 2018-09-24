using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Domain.Service
{
    public abstract class BaseDomainSvc
    {
        /// <summary>
        /// AutoMapperInterface
        /// </summary>
        protected IMapper _mapper;

        /// <summary>
        /// MainDbContext
        /// </summary>
        protected TestDBContext _testDB { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="testDB"></param>
        protected BaseDomainSvc(IMapper mapper, TestDBContext testDB) 
        {
            _mapper = mapper;
            _testDB = testDB;
        }
    }
}
