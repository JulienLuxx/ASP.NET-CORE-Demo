using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Domain;

namespace Test.Service.Impl
{
    public abstract class BaseSvc
    {
        /// <summary>
        /// AutoMapperInterface
        /// </summary>
        protected readonly IMapper _mapper;

        //protected TestDBContext TestDB = new TestDBContext();

        /// <summary>
        /// MainDbContext
        /// </summary>
        protected TestDBContext _testDB { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="testDB"></param>
        protected BaseSvc(IMapper mapper,TestDBContext testDB)
        {
            _mapper = mapper;
            _testDB = testDB;
        }
    }
}
