namespace Klaims.Scim.Query.Filter
{
	public abstract class FilterNode
	{
		protected FilterNode(Operator filterOperator)
		{
			this.Operator = filterOperator;
		}

		public Operator Operator { get; private set; }
	}
}
