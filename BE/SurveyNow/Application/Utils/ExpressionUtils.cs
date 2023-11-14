using Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public static class ExpressionUtils
    {
        public static Expression ToQueryable<T>(Expression source)
        {
            var method = typeof(Queryable).GetMethods()
                .Where(m => m.Name.Equals("AsQueryable"))
                .Single(m => m.IsGenericMethod)
                .MakeGenericMethod(typeof(T));
            var expression = Expression.Call(method: method, arg0: source);
            return expression;
        }

        public static Expression Any<T>(Expression queryableSource, Expression<Func<T, bool>>? predicate = null)
        {
            if(predicate != null)
            {
                var method = typeof(Queryable).GetMethods()
                    .Where(method => method.Name.Equals("Any"))
                    .Single(method => method.GetParameters().Count() == 2)
                    .MakeGenericMethod(typeof(T));
                var expression = Expression.Call(method: method, arg0: queryableSource, arg1: predicate);
                return expression;
            }
            else
            {
                var method = typeof(Queryable).GetMethods()
                    .Where(method => method.Name.Equals("Any"))
                    .Single(method => method.GetParameters().Count() == 1)
                    .MakeGenericMethod(typeof(T));
                var expression = Expression.Call(method: method, arg0: queryableSource);
                return expression;
            }
        }

        public static Expression ContainsString<T>(Expression stringInstance, Expression stringValue)
        {
            string a = ""; a.Contains(""); 
            var method = typeof(String).GetMethod("Contains", new Type[] { typeof(string) });
            var expression = Expression.Call(instance: stringInstance, method: method, stringValue);
            return expression;
            //var parameterExp = Expression.Parameter(typeof(T), "type");
            //var propertyExp = Expression.Property(parameterExp, propertyName);
            //MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            //var someValue = Expression.Constant(propertyValue, typeof(string));
            //var containsMethodExp = Expression.Call(propertyExp, method, someValue);
            //return containsMethodExp;
        }

        public static Expression ForEach(Expression collection, ParameterExpression loopVar, Expression loopContent)
        {
            var elementType = loopVar.Type;
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);
            var enumeratorType = typeof(IEnumerator<>).MakeGenericType(elementType);

            var enumeratorVar = Expression.Variable(enumeratorType, "enumerator");
            var getEnumeratorCall = Expression.Call(collection, enumerableType.GetMethod("GetEnumerator"));
            var enumeratorAssign = Expression.Assign(enumeratorVar, getEnumeratorCall);

            // The MoveNext method's actually on IEnumerator, not IEnumerator<T>
            var moveNextCall = Expression.Call(enumeratorVar, typeof(IEnumerator).GetMethod("MoveNext"));

            var breakLabel = Expression.Label("LoopBreak");

            var loop = Expression.Block(new[] { enumeratorVar },
                enumeratorAssign,
                Expression.Loop(
                    Expression.IfThenElse(
                        Expression.Equal(moveNextCall, Expression.Constant(true)),
                        Expression.Block(new[] { loopVar },
                            Expression.Assign(loopVar, Expression.Property(enumeratorVar, "Current")),
                            loopContent
                        ),
                        Expression.Break(breakLabel)
                    ),
                breakLabel)
            );

            return loop;
        }

        public static Expression For(ParameterExpression loopVar, Expression initValue, Expression condition, Expression increment, Expression loopContent)
        {
            var initAssign = Expression.Assign(loopVar, initValue);

            var breakLabel = Expression.Label("LoopBreak");

            var loop = Expression.Block(new[] { loopVar },
                initAssign,
                Expression.Loop(
                    Expression.IfThenElse(
                        condition,
                        Expression.Block(
                            loopContent,
                            increment
                        ),
                        Expression.Break(breakLabel)
                    ),
                breakLabel)
            );

            return loop;
        }
    }
}