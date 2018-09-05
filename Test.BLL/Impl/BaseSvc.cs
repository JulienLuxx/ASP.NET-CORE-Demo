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
        protected IMapper _mapper;

        //protected TestDBContext TestDB = new TestDBContext();

        protected TestDBContext _testDB { get; set; }

        /// <summary>
        /// BaseSvc.Ctor
        /// </summary>
        /// <param name="mapper">AutoMapperInjectionParam</param>
        /// <param name="testDB">DbContextInjectionParam</param>
        protected BaseSvc(IMapper mapper,TestDBContext testDB)
        {
            _mapper = mapper;
            _testDB = testDB;
        }
    }
}
