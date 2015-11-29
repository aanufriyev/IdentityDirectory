// Ported from eSCIMo under ASL 2.0 

namespace IdentityDirectory.Scim.Query
{
    #region

    using System;
    using System.Text;

    #endregion

    public class LogicalExpression : FilterExpresstion
    {
        public LogicalExpression(ExpresssionOperator filterOperator)
            : base(filterOperator)
        {
        }

        public bool HasBothChildren => Left != null && Right != null;

        public FilterExpresstion Left { get; private set; }

        public FilterExpresstion Right { get; private set; }

        public void AddNode(FilterExpresstion node)
        {
            if (Left == null)
            {
                Left = node;
            }
            else if (Right == null)
            {
                Right = node;
            }
            else
            {
                throw new InvalidOperationException("A logical expression can only hold two nodes");
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("(");
            if (Left != null)
            {
                sb.Append(Left);
            }

            sb.Append(Operator.Equals(ExpresssionOperator.And) ? " AND " : " OR ");

            if (Right != null)
            {
                sb.Append(Right);
            }
            sb.Append(")");
            return sb.ToString();
        }
    }
}