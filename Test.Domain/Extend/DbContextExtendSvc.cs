using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Test.Domain.Entity;

namespace Test.Domain.Extend
{
    public class DbContextExtendSvc<TEntity>where TEntity:class,IEntity
    {

        //public void Update(DbContext dbContext, TEntity newEntity)
        //{
        //    if (null == newEntity)
        //    {
        //        throw new ArgumentNullException(nameof(newEntity));
        //    }
        //    var oldEntity = dbContext.Find(newEntity.)
        //}

        public void Update(DbContext dbContext, TEntity newEntity, TEntity oldEntity)
        {
            if (null == newEntity)
            {
                throw new ArgumentNullException(nameof(newEntity));
            }
            if (null == oldEntity)
            {
                throw new ArgumentNullException(nameof(oldEntity));
            }
            ValidateVersion(newEntity, oldEntity);
            dbContext.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// 验证时间戳
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newEntity"></param>
        /// <param name="oldEntity"></param>
        public void ValidateVersion<T>(T newEntity, T oldEntity) where T : IEntity
        {
            if (null == newEntity.Timestamp)
            {
                throw new DBConcurrencyException();
            }
            for (int i = 0; i < oldEntity.Timestamp.Length; i++)
            {
                if (newEntity.Timestamp[i] != oldEntity.Timestamp[i])
                {
                    throw new DBConcurrencyException();
                }
            }
        }
    }
}
