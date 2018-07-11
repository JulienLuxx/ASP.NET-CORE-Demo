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

        protected TestDBContext TestDB = new TestDBContext();

        protected BaseSvc(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
