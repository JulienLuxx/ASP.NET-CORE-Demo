using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace Test.XUnitTest.Infrastructure
{
    public static class EntityFrameworkSetupData
    {
        public static Mock<DbSet<T>> SetupList<T>(this Mock<DbSet<T>> mockSet, List<T> list) where T : class
        {
            return mockSet.SetupArray(list.ToArray());
        }

        public static Mock<DbSet<T>> SetupArray<T>(this Mock<DbSet<T>> mockSet,params T[] array)where T : class
        {
            var queryable = array.AsQueryable();
            mockSet.As<IAsyncEnumerable<T>>().Setup(x => x.GetEnumerator()).Returns(new UnitTestAsyncEnumerator<T>(queryable.GetEnumerator()));
            mockSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(new UnitTestAsyncQueryProvider<T>(queryable.Provider));
            mockSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            return mockSet;
        }
    }
}
