namespace IdentityDirectory.Scim.Query
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Klaims.Framework.Utility;
    using Services;

    public class DefaultFilterBinder : IFilterBinder
    {
        public Expression<Func<TResource, bool>> Bind<TResource>(FilterExpresstion filter, string sortBy, bool ascending, IAttributeNameMapper mapper = null)
        {
            return this.BuildExpression<TResource>(filter, mapper, null);
        }

        private Expression<Func<TResource, bool>> BuildExpression<TResource>(FilterExpresstion filter, IAttributeNameMapper mapper, string prefix)
        {
            var logicalExpression = filter as LogicalExpression;
            if (logicalExpression != null)
            {
                return this.BindLogicalExpression<TResource>(filter, mapper, prefix, logicalExpression);
            }

            var valueExpression = filter as ValueExpression;
            if (valueExpression != null)
            {
                return this.BindValueExpression<TResource>(filter, mapper, valueExpression);
            }

            throw new InvalidOperationException("Unknown node type");
        }

        protected virtual Expression<Func<TResource, bool>> BindValueExpression<TResource>(FilterExpresstion filter, IAttributeNameMapper mapper, ValueExpression expression)
        {
            var parameter = Expression.Parameter(typeof(TResource));
            var property = Expression.Property(parameter, mapper.MapToInternal(expression.Attribute));

            Expression binaryExpression = null;
            if (filter.Operator.Equals(ExpresssionOperator.Eq))
            {
                // Workaround for missing coersion between String and Guid types.
                var propertyValue = property.Type == typeof(Guid) ? (object)Guid.Parse(expression.Value) : expression.Value;
                binaryExpression = Expression.Equal(property, Expression.Convert(Expression.Constant(propertyValue), property.Type));
            }
            else if (filter.Operator.Equals(ExpresssionOperator.Co))
            {
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                binaryExpression = Expression.Call(property, method, Expression.Constant(expression.Value, typeof(string)));
            }
            else if (filter.Operator.Equals(ExpresssionOperator.Gt))
            {
                binaryExpression = Expression.GreaterThan(property, Expression.Convert(Expression.Constant(expression.Value), property.Type));
            }
            else if (filter.Operator.Equals(ExpresssionOperator.Ge))
            {
                binaryExpression = Expression.GreaterThanOrEqual(property, Expression.Convert(Expression.Constant(expression.Value), property.Type));
            }
            else if (filter.Operator.Equals(ExpresssionOperator.Lt))
            {
                binaryExpression = Expression.LessThan(property, Expression.Convert(Expression.Constant(expression.Value), property.Type));
            }
            else if (filter.Operator.Equals(ExpresssionOperator.Le))
            {
                binaryExpression = Expression.LessThanOrEqual(property, Expression.Convert(Expression.Constant(expression.Value), property.Type));
            }
            else if (filter.Operator.Equals(ExpresssionOperator.Pr))
            {
                // We need to counter guid and other non nullable value types. 
                // If value cannot be null, then it is always present.
                if (IsNullable(property.Type))
                {
                    binaryExpression = Expression.NotEqual(property, Expression.Constant(null, property.Type));
                }
                else
                {
                    binaryExpression = Expression.Constant(true);
                }
            }
            if (binaryExpression == null)
            {
                throw new InvalidOperationException("Unsupported node operator");
            }

            return Expression.Lambda<Func<TResource, bool>>(binaryExpression, parameter);
        }

        protected virtual Expression<Func<TResource, bool>> BindLogicalExpression<TResource>(
            FilterExpresstion filter,
            IAttributeNameMapper mapper,
            string prefix,
            LogicalExpression expression)
        {
            var leftNodeExpression = this.BuildExpression<TResource>(expression.Left, mapper, prefix);
            var rightNodeExpression = this.BuildExpression<TResource>(expression.Right, mapper, prefix);
            if (filter.Operator.Equals(ExpresssionOperator.And))
            {
                return leftNodeExpression.And(rightNodeExpression);
            }
            if (filter.Operator.Equals(ExpresssionOperator.Or))
            {
                return leftNodeExpression.Or(rightNodeExpression);
            }
            throw new InvalidOperationException("Unsupported branch operator");
        }

        // TODO: Move to extensions
        private static bool IsNullable(Type type)
        {
            if (!type.GetTypeInfo().IsValueType) return true; // ref-type
            return Nullable.GetUnderlyingType(type) != null;
        }
    }
}