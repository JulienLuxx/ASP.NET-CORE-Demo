using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Domain.Entity;

namespace Test.Domain.Extend
{
    public interface IDbContextExtendSvc<TEntity> where TEntity : class, IEntity
    {
        void Update(DbContext dbContext, TEntity newEntity, TEntity oldEntity);
    }

    public interface IDbContextExtendSvc
    {

    }
}
