using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Entity;

namespace Test.Domain.Extend
{
    public interface IDbContextExtendSvc<TEntity> where TEntity : class, IEntity
    {
        void Update(DbContext dbContext, TEntity newEntity, TEntity oldEntity);
    }

    public interface IDbContextExtendSvc
    {
        Task<int> CommitTestAsync<TDbContext, TEntity>(TDbContext dbContext, bool clientWin = false) where TDbContext : DbContext where TEntity : IEntity;
    }
}
