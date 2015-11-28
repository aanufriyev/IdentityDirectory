namespace Klaims.Scim.Query
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Klaims.Framework.Utility;
    using Klaims.Scim.Query.Filter;
    using Klaims.Scim.Services;

    public class DefaultFilterBinder : IFilterBinder
    {
        public Expression<Func<TResource, bool>> Bind<TResource>(FilterNode filterNode, string sortBy, bool @ascending, IAttributeNameMapper mapper = null)
        {
            return this.BuildExpression<TResource>(filterNode, mapper, null);
        }

        private Expression<Func<TResource, bool>> BuildExpression<TResource>(FilterNode filter, IAttributeNameMapper mapper, string prefix)
        {
            var branchNode = filter as BranchNode;
            var terminalNode = filter as TerminalNode;

            if (branchNode != null)
            {
                return this.BindBranchNode<TResource>(filter, mapper, prefix, branchNode);
            }

            if (terminalNode != null)
            {
                return this.BindTerminalNode<TResource>(filter, mapper, terminalNode);
            }

            throw new InvalidOperationException("Unknown node type");
        }

        protected virtual Expression<Func<TResource, bool>> BindTerminalNode<TResource>(FilterNode filter, IAttributeNameMapper mapper, TerminalNode terminalNode)
        {
            var parameter = Expression.Parameter(typeof(TResource));
            var property = Expression.Property(parameter, mapper.MapToInternal(terminalNode.Attribute));

            Expression expression = null;
            if (filter.Operator.Equals(Operator.Eq))
            {
                // Workaround for missing coersion between String and Guid types.
                var propertyValue = property.Type == typeof(Guid) ? (object)Guid.Parse(terminalNode.Value) : terminalNode.Value;
                expression = Expression.Equal(property, Expression.Convert(Expression.Constant(propertyValue), property.Type));
            }
            else if (filter.Operator.Equals(Operator.Co))
            {
                var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                expression = Expression.Call(property, method, Expression.Constant(terminalNode.Value, typeof(string)));
            }
            else if (filter.Operator.Equals(Operator.Gt))
            {
                expression = Expression.GreaterThan(property, Expression.Convert(Expression.Constant(terminalNode.Value), property.Type));
            }
            else if (filter.Operator.Equals(Operator.Ge))
            {
                expression = Expression.GreaterThanOrEqual(property, Expression.Convert(Expression.Constant(terminalNode.Value), property.Type));
            }
            else if (filter.Operator.Equals(Operator.Lt))
            {
                expression = Expression.LessThan(property, Expression.Convert(Expression.Constant(terminalNode.Value), property.Type));
            }
            else if (filter.Operator.Equals(Operator.Le))
            {
                expression = Expression.LessThanOrEqual(property, Expression.Convert(Expression.Constant(terminalNode.Value), property.Type));
            }
            else if (filter.Operator.Equals(Operator.Pr))
            {
                // We need to counter guid and other non nullable value types. 
                // If value cannot be null, then it is always present.
                if (IsNullable(property.Type))
                {
                    expression = Expression.NotEqual(property, Expression.Constant(null, property.Type));
                }
                else
                {
                    expression = Expression.Constant(true);
                }
            }
            if (expression == null)
            {
                throw new InvalidOperationException("Unsupported node operator");
            }

            return Expression.Lambda<Func<TResource, bool>>(expression, parameter);
        }

        protected virtual Expression<Func<TResource, bool>> BindBranchNode<TResource>(
            FilterNode filter,
            IAttributeNameMapper mapper,
            string prefix,
            BranchNode branchNode)
        {
            var leftNodeExpression = this.BuildExpression<TResource>(branchNode.Left, mapper, prefix);
            var rightNodeExpression = this.BuildExpression<TResource>(branchNode.Right, mapper, prefix);
            if (filter.Operator.Equals(Operator.And))
            {
                return leftNodeExpression.And(rightNodeExpression);
            }
            if (filter.Operator.Equals(Operator.Or))
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