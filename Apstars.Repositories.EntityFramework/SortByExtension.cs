using Apstars.Querying;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Apstars.Repositories.EntityFramework
{
    /// <summary>
    /// Represents the method extension for the sorting of entity framework repository.
    /// </summary>
    internal static class SortByExtension
    {
        #region Internal Methods

        internal static IOrderedQueryable<TEntity> SortBy<TKey, TEntity>(
            this IQueryable<TEntity> query,
            Expression<Func<TEntity, dynamic>> sortPredicate)
            where TKey : IEquatable<TKey>
            where TEntity : class, IAggregateRoot<TKey>
        {
            return InvokeSortBy<TKey, TEntity>(query, sortPredicate, SortOrder.Ascending);
        }

        internal static IOrderedQueryable<TEntity> SortByDescending<TKey, TEntity>(
            this IQueryable<TEntity> query,
            Expression<Func<TEntity, dynamic>> sortPredicate)
            where TKey : IEquatable<TKey>
            where TEntity : class, IAggregateRoot<TKey>
        {
            return InvokeSortBy<TKey, TEntity>(query, sortPredicate, SortOrder.Descending);
        }

        internal static IOrderedQueryable<TEntity> SortThenBy<TKey, TEntity>(
            this IOrderedQueryable<TEntity> query,
            Expression<Func<TEntity, dynamic>> sortPredicate)
            where TKey : IEquatable<TKey>
            where TEntity : class, IAggregateRoot<TKey>
        {
            return InvokeSortThenBy<TKey, TEntity>(query, sortPredicate, SortOrder.Ascending);
        }

        internal static IOrderedQueryable<TEntity> SortThenByDescending<TKey, TEntity>(
            this IOrderedQueryable<TEntity> query,
            Expression<Func<TEntity, dynamic>> sortPredicate)
            where TKey : IEquatable<TKey>
            where TEntity : class, IAggregateRoot<TKey>
        {
            return InvokeSortThenBy<TKey, TEntity>(query, sortPredicate, SortOrder.Descending);
        }
        #endregion

        #region Private Methods

        private static IOrderedQueryable<TEntity> InvokeSortBy<TKey, TEntity>(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, dynamic>> sortPredicate,
            SortOrder sortOrder)
            where TKey : IEquatable<TKey>
            where TEntity : class, IAggregateRoot<TKey>
        {
            var param = sortPredicate.Parameters[0];
            string propertyName = null;
            Type propertyType = null;
            Expression bodyExpression = null;
            if (sortPredicate.Body is UnaryExpression)
            {
                UnaryExpression unaryExpression = sortPredicate.Body as UnaryExpression;
                bodyExpression = unaryExpression.Operand;
            }
            else if (sortPredicate.Body is MemberExpression)
            {
                bodyExpression = sortPredicate.Body;
            }
            else throw new ArgumentException(@"The body of the sort predicate expression should be 
                either UnaryExpression or MemberExpression.", "sortPredicate");
            MemberExpression memberExpression = (MemberExpression)bodyExpression;
            propertyName = memberExpression.Member.Name;
            if (memberExpression.Member.MemberType == MemberTypes.Property)
            {
                PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
                propertyType = propertyInfo.PropertyType;
            }
            else throw new InvalidOperationException(@"Cannot evaluate the type of property since the member expression 
                represented by the sort predicate expression does not contain a PropertyInfo object.");

            Type funcType = typeof(Func<,>).MakeGenericType(typeof(TEntity), propertyType);
            LambdaExpression convertedExpression = Expression.Lambda(
                funcType,
                Expression.Convert(Expression.Property(param, propertyName), propertyType),
                param);

            var sortingMethods = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static);
            var sortingMethodName = GetSortingMethodName(sortOrder);
            var sortingMethod =
                sortingMethods.Where(
                    sm => sm.Name == sortingMethodName && sm.GetParameters() != null && sm.GetParameters().Length == 2)
                    .First();
            return
                (IOrderedQueryable<TEntity>)
                sortingMethod.MakeGenericMethod(typeof(TEntity), propertyType)
                    .Invoke(null, new object[] { query, convertedExpression });
        }

        private static string GetSortingMethodName(SortOrder sortOrder)
        {
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return "OrderBy";
                case SortOrder.Descending:
                    return "OrderByDescending";
                default:
                    throw new ArgumentException(
                        "Sort Order must be specified as either Ascending or Descending.",
                        "sortOrder");
            }
        }

        private static IOrderedQueryable<TEntity> InvokeSortThenBy<TKey, TEntity>(
            IOrderedQueryable<TEntity> query,
            Expression<Func<TEntity, dynamic>> sortPredicate,
            SortOrder sortOrder)
            where TKey : IEquatable<TKey>
            where TEntity : class, IAggregateRoot<TKey>
        {
            var param = sortPredicate.Parameters[0];
            string propertyName = null;
            Type propertyType = null;
            Expression bodyExpression = null;
            if (sortPredicate.Body is UnaryExpression)
            {
                UnaryExpression unaryExpression = sortPredicate.Body as UnaryExpression;
                bodyExpression = unaryExpression.Operand;
            }
            else if (sortPredicate.Body is MemberExpression)
            {
                bodyExpression = sortPredicate.Body;
            }
            else throw new ArgumentException(@"The body of the sort predicate expression should be 
                either UnaryExpression or MemberExpression.", "sortPredicate");
            MemberExpression memberExpression = (MemberExpression)bodyExpression;
            propertyName = memberExpression.Member.Name;
            if (memberExpression.Member.MemberType == MemberTypes.Property)
            {
                PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
                propertyType = propertyInfo.PropertyType;
            }
            else throw new InvalidOperationException(@"Cannot evaluate the type of property since the member expression 
                represented by the sort predicate expression does not contain a PropertyInfo object.");

            Type funcType = typeof(Func<,>).MakeGenericType(typeof(TEntity), propertyType);
            LambdaExpression convertedExpression = Expression.Lambda(
                funcType,
                Expression.Convert(Expression.Property(param, propertyName), propertyType),
                param);

            var sortingMethods = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static);
            var sortingMethodName = GetSortingThenMethodName(sortOrder);
            var sortingMethod =
                sortingMethods.Where(
                    sm => sm.Name == sortingMethodName && sm.GetParameters() != null && sm.GetParameters().Length == 2)
                    .First();
            return
                (IOrderedQueryable<TEntity>)
                sortingMethod.MakeGenericMethod(typeof(TEntity), propertyType)
                    .Invoke(null, new object[] { query, convertedExpression });
        }

        private static string GetSortingThenMethodName(SortOrder sortOrder)
        {
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return "ThenBy";
                case SortOrder.Descending:
                    return "ThenByDescending";
                default:
                    throw new ArgumentException(
                        "Sort Order must be specified as either Ascending or Descending.",
                        "sortOrder");
            }
        }

        #endregion
    }
}
