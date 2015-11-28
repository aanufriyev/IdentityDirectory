// Ported from eSCIMo under ASL 2.0 

namespace Klaims.Scim.Query.Filter
{
	using System;
	using System.Text;

	public class BranchNode : FilterNode
	{
		public BranchNode(Operator filterOperator)
			: base(filterOperator)
		{
		}

		public bool HasBothChildren => this.Left != null && this.Right != null;

		public FilterNode Left { get; private set; }

		public FilterNode Right { get; private set; }

		public void AddNode(FilterNode node)
		{
			if (this.Left == null)
			{
				this.Left = node;
			}
			else if (this.Right == null)
			{
				this.Right = node;
			}
			else
			{
				throw new InvalidOperationException("A branch expression can only hold two nodes");
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append("(");
			if (this.Left != null)
			{
				sb.Append(this.Left);
			}

			sb.Append(this.Operator.Equals(Operator.And) ? " AND " : " OR ");

			if (this.Right != null)
			{
				sb.Append(this.Right);
			}
			sb.Append(")");
			return sb.ToString();
		}
	}
}