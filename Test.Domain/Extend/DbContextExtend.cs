using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Domain.Entity;

namespace Test.Domain.Extend
{
    public static class DbContextExtend
    {
        public static void Update(DbContext dbContext, IEntity newEntity, IEntity oldEntity)
        {
            if (null == newEntity)
            {
                throw new ArgumentNullException(nameof(newEntity));
            }
            if (null == oldEntity)
            {
                throw new ArgumentNullException(nameof(oldEntity));
            }
            try
            {
                ValidateVersion(newEntity, oldEntity);
            }
            catch (DBConcurrencyException ex)
            {
                dbContext.Entry(oldEntity).CurrentValues.SetValues(newEntity);

            }
        }

        public static void ValidateVersion<TEntity>(TEntity newEntity, TEntity oldEntity) where TEntity : IEntity
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

        public static async Task MSDNCommitAsync<TDbContext,TEntity>(TDbContext dbContext/*, TEntity newEntity, TEntity oldEntity*/) where TDbContext : DbContext where TEntity:IEntity
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
                        //return flag;
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is TEntity)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = await entry.GetDatabaseValuesAsync();
                            var n = proposedValues["Timestamp"];
                            var o = databaseValues["Timestamp"];
                            if(proposedValues["Timestamp"] != databaseValues["Timestamp"])
                            {
                                foreach (var property in proposedValues.Properties)
                                {
                                    if (!property.Name.Equals("Timestamp"))
                                    {
                                        var proposedValue = proposedValues[property];
                                        var databaseValue = databaseValues[property];
                                        databaseValue = proposedValue;
                                    }
                                }
                                entry.OriginalValues.SetValues(databaseValues);
                            }
                        }
                        else
                        {
                            throw new NotSupportedException("Don't know how to handle concurrency conflicts for " + entry.Metadata.Name);
                        }
                    }
                }
            }
            //await dbContext.SaveChangesAsync();
        }

        public static async Task GitHubCommitAsync<TDbContext, TEntity>(TDbContext dbContext/*, TEntity newEntity, TEntity oldEntity*/) where TDbContext : DbContext where TEntity : IEntity
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
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is TEntity)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = await entry.GetDatabaseValuesAsync();
                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];
                                databaseValue = proposedValue;
                            }
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException("Don't know how to handle concurrency conflicts for " + entry.Metadata.Name);
                        }
                    }
                }
            }
        }

        public static async Task<int> CommitTestAsync<TDbContext, TEntity>(TDbContext dbContext,bool forceSave=false) where TDbContext : DbContext where TEntity : IEntity
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
                            if (forceSave)
                            {
                                await entry.ReloadAsync();
                            }
                            var originalValues = entry.OriginalValues;
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = await entry.GetDatabaseValuesAsync();
                            var originalTimestamp = originalValues["Timestamp"];
                            var proposedTimestamp = proposedValues["Timestamp"];
                            var databaseTimestamp = databaseValues["Timestamp"];
                            var oInt = int.Parse(ConvertToTimeSpanString(originalTimestamp), NumberStyles.HexNumber);
                            var pInt = int.Parse(ConvertToTimeSpanString(proposedTimestamp), NumberStyles.HexNumber);
                            var dInt = int.Parse(ConvertToTimeSpanString(databaseTimestamp), NumberStyles.HexNumber);
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

        public static string ConvertToTimeSpanString(object obj)
        {
            var btTsArray = obj as byte[];
            var str = BitConverter.ToString(btTsArray);
            str = BitConverter.ToString(btTsArray).Replace("-", "");
            return str;
        }
    }
}
