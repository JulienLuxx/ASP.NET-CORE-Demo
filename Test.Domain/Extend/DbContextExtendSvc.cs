﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
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
        //    var oldEntity=dbContext.Find(newEntity.)
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

    public class DbContextExtendSvc: IDbContextExtendSvc
    {
        public string ConvertToTimeSpanString(object obj)
        {
            var btTsArray = obj as byte[];
            var str = BitConverter.ToString(btTsArray);
            str = BitConverter.ToString(btTsArray).Replace("-", "");
            return str;
        }

        public DateTime ConvertToDateTime(byte[] bytes, int offset=0)
        {            
            if (null != bytes)
            {
                var ticks = BitConverter.ToInt64(bytes, offset);
                if (ticks < DateTime.MaxValue.Ticks && ticks > DateTime.MinValue.Ticks)
                {
                    var dt = new DateTime(ticks);
                    return dt;
                }
            }
            return new DateTime();
        }

        public async Task<int> CommitTestAsync<TDbContext, TEntity>(TDbContext dbContext, bool clientWin = false) where TDbContext : DbContext where TEntity : IEntity
        {
            var saved = false;
            while (!saved)
            {
                try
                {
                    var flag = await dbContext.SaveChangesAsync();
                    if (flag > 0)
                    {
                        saved = true;
                        return flag;
                    }
                    return 0;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        //var c = entry.CurrentValues["Timestamp"];
                        //var dv = await entry.GetDatabaseValuesAsync();
                        //var d = dv["Timestamp"];
                        //var o = entry.OriginalValues["Timestamp"];
                        //await entry.ReloadAsync();
                        //var tmp = entry.OriginalValues["Timestamp"];
                        if (entry.Entity is TEntity)
                        {
                            if (clientWin)
                            {
                                await entry.ReloadAsync();
                            }
                            var originalValues = entry.OriginalValues;
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = await entry.GetDatabaseValuesAsync();
                            var originalTimestamp = originalValues["Timestamp"];
                            var proposedTimestamp = proposedValues["Timestamp"];
                            var databaseTimestamp = databaseValues["Timestamp"];
                            var oInt = long.Parse(ConvertToTimeSpanString(originalTimestamp), NumberStyles.HexNumber);
                            var pInt = long.Parse(ConvertToTimeSpanString(proposedTimestamp), NumberStyles.HexNumber);
                            var dInt = long.Parse(ConvertToTimeSpanString(databaseTimestamp), NumberStyles.HexNumber);
                            if (oInt >= dInt)
                            {
                                foreach (var property in proposedValues.Properties)
                                {
                                    var proposedValue = proposedValues[property];
                                    var databaseValue = databaseValues[property];
                                    databaseValue = proposedValue;
                                }
                                entry.OriginalValues.SetValues(databaseValues);
                                return 1;
                            }
                            else
                            {
                                return -1;
                            }
                        }
                        else
                        {
                            throw new NotSupportedException("Don't know how to handle concurrency conflicts for " + entry.Metadata.Name);
                        }
                    }
                }
            }
            return 0;
        }

    }
}
