using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public static class ExpressionUtils
    {
        public static Expression ToQueryable<T>(string propertyName)
        {
            var method = typeof(Queryable).GetMethods()
                .Where(m => m.Name.Equals("AsQueryable"))
                .Single(m => m.IsGenericMethod)
                .MakeGenericMethod(typeof(T));
            var expression = Expression.Call(method: method, arg0: Expression.Property(Expression.Parameter(typeof(T)), propertyName));
            return expression;
        }

        public static Expression Any<T>(Expression queryableSource, Expression<Func<T, bool>> predicate)
        {
            var method = typeof(Queryable).GetMethods()
                .Where(method => method.Name.Equals("Any"))
                .Single(method => method.GetParameters().Count() == 2)
                .MakeGenericMethod(typeof(T));
            var expression = Expression.Call(method: method, arg0: queryableSource, arg1: predicate);
            return expression;
        }
    }
}